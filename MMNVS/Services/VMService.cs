#nullable disable
using Lextm.SharpSnmpLib.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMNVS.Data;
using MMNVS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Management.Automation.Runspaces;
using System.Net;

namespace MMNVS.Services
{
    public class VMService : IVMService
    {
        private readonly IDbService _dbService;

        public VMService(IDbService dbService)
        {
            _dbService = dbService;
        }

        private string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public async Task<string> GetvCenterApiKey()
        {
            string url = _dbService.GetSettings().vCenterApiUrl + "/com/vmware/cis/session";
            string username = _dbService.GetSettings().vCenterUsername;
            string password = _dbService.GetSettings().vCenterPassword;
            string header = EncodeTo64(username + ":" + password);

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", "Basic " + header);
                var response = await httpClient.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                if(htmlAttributes != null) return htmlAttributes["value"];
                return null;
            }
        }

        public void GetVirtualServersFromvCenter()
        {
            IEnumerable<VirtualServer> virtualServers = _dbService.GetVirtualServers();
            string apiKey = GetvCenterApiKey().Result;
            string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm";
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("vmware-api-session-id", apiKey);
                using (var response = (httpClient.SendAsync(request)))
                {
                    string jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                    JObject data = JObject.Parse(jsonResponse);
                    IList<JToken> results = data["value"].Children().ToList();
                    foreach (JToken result in results)
                    {
                        VirtualServer virtualServer = result.ToObject<VirtualServer>();

                        if (_dbService.IsVirtualStorageServer(virtualServer.Name) == false) //kontrola, zda se nejedná o virtuální storage server, který mezi tyto virtuální servery nepatří
                        {
                            if (result.Value<string>("power_state") == "POWERED_ON") virtualServer.StartServerOnStart = true;
                            VirtualServer virtualServerDB = virtualServers.FirstOrDefault(v => v.VMId == virtualServer.VMId);
                            if (virtualServerDB == null)
                            {
                                if (virtualServer.Name == "vcenter") _dbService.AddVirtualServer(virtualServer, true);
                                else _dbService.AddVirtualServer(virtualServer);
                            }
                            else if (virtualServerDB.VMId == virtualServerDB.VMId && virtualServerDB.Name != virtualServer.Name)
                            {
                                virtualServerDB.Name = virtualServer.Name;
                                _dbService.EditItem(virtualServerDB);
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<string, PowerStateEnum> GetVirtualServersPower()
        {
            try
            {
                Dictionary<string, PowerStateEnum> powerStates = new Dictionary<string, PowerStateEnum>();
                string apiKey = GetvCenterApiKey().Result;
                string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm";
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                    request.Headers.Add("vmware-api-session-id", apiKey);
                    using (var response = (httpClient.SendAsync(request)))
                    {
                        string jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                        JObject data = JObject.Parse(jsonResponse);
                        IList<JToken> results = data["value"].Children().ToList();
                        foreach (JToken result in results)
                        {
                            string powerState = result.Value<string>("power_state");
                            string vmId = result.Value<string>("vm");
                            if (powerState == "POWERED_ON") powerStates.Add(vmId, PowerStateEnum.PoweredOn);
                            else powerStates.Add(vmId, PowerStateEnum.Unknown);
                        }
                    }
                }
                return powerStates;
            }
            catch
            {
                return new Dictionary<string, PowerStateEnum>();
            }
        }
        public List<JToken> GetVirtualServerFromvCenter(string vmId)
        {
            string apiKey = GetvCenterApiKey().Result;
            string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm/" + vmId;
            List<JToken> results = new List<JToken>();
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("vmware-api-session-id", apiKey);
                using (var response = (httpClient.SendAsync(request)))
                {
                    string jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                    JObject data = JObject.Parse(jsonResponse);
                    results = data["value"].Children().ToList();
                }
            }
            return results;
        }

        public async Task<HttpStatusCode> StartVirtualServer(string vmId)
        {
            string apiKey = GetvCenterApiKey().Result;
            string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm/" + vmId + "/power/start";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Headers.Add("vmware-api-session-id", apiKey);
                var response = await httpClient.SendAsync(request);
                return response.StatusCode;
            }
        }

        public async void ShutdownVirtualServer(string vmId)
        {
            var version = _dbService.GetSettingsWithoutInclude().vCenterVersion;
            if (version == vCenterVersionEnum.v65)
            {
                ShutdownVirtualServer65(vmId);
            }
            else if (version == vCenterVersionEnum.v67)
            {
                await ShutdownVirtualServer67(vmId);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        public async Task<HttpStatusCode> ShutdownVirtualServer67(string vmId) //vypínání ve verzi 6.7 a novější (pomocí REST API)
        {
            string apiKey = GetvCenterApiKey().Result;
            string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm/" + vmId + "/guest/power?action=shutdown";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Headers.Add("vmware-api-session-id", apiKey);
                var response = await httpClient.SendAsync(request);
                return response.StatusCode;
            }
        }

        public void ShutdownVirtualServer65(string vmId) //Vypínání ve verzi 6.6 a starší, v REST Api není endpoint pro shutdown virtuálního serveru
        {
            VirtualServer virtualServer = _dbService.GetVirtualServer(vmId);
            AppSettings settings = _dbService.GetSettingsWithoutInclude();
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core");
            pipeline.Commands.AddScript("Connect-VIServer -Server " + settings.vCenterIP + " -Protocol https -User " + settings.vCenterUsername + " -Password " + settings.vCenterPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VM -Name " + virtualServer.Name + " | Shutdown-VMGuest -Confirm:$false");
            pipeline.Invoke();
            runspace.Close();
        }

        public PowerStateEnum GetPowerVirtualServer(string vmId)
        {
            try
            {
                string apiKey = GetvCenterApiKey().Result;
                string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm/" + vmId + "/power";
                List<JToken> results = new List<JToken>();
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                    request.Headers.Add("vmware-api-session-id", apiKey);
                    using (var response = (httpClient.SendAsync(request)))
                    {
                        string jsonResponse = response.Result.Content.ReadAsStringAsync().Result;
                        JObject data = JObject.Parse(jsonResponse);
                        results = data["value"].Children().ToList();
                    }
                }
            string ps = results[0].ToString();
            if (ps == "\"state\": \"POWERED_ON\"") return PowerStateEnum.PoweredOn;
            else if (ps == "\"state\": \"POWERED_OFF\"") return PowerStateEnum.PoweredOff;
            else return PowerStateEnum.Unknown;
            }
            catch
            {
                return PowerStateEnum.Unknown;
            }
        }

        public PowerStateEnum GetvCenterState()
        {
            try
            {
                if (GetvCenterApiKey().Result != null) return PowerStateEnum.PoweredOn;
                else return PowerStateEnum.Unknown;
            }
            catch
            {
                return PowerStateEnum.Unknown;
            }
        }
    }
    public interface IVMService
    {
        Task<string> GetvCenterApiKey();
        void GetVirtualServersFromvCenter();
        List<JToken> GetVirtualServerFromvCenter(string VMId);
        Task<HttpStatusCode> StartVirtualServer(string vmId);
        void ShutdownVirtualServer(string vmId);
        PowerStateEnum GetPowerVirtualServer(string vmId);
        PowerStateEnum GetvCenterState();
        Dictionary<string, PowerStateEnum> GetVirtualServersPower();
    }
}
