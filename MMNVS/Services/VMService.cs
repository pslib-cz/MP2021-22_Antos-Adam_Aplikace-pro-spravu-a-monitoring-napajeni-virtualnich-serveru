﻿using Lextm.SharpSnmpLib.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMNVS.Data;
using MMNVS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MMNVS.Services
{
    public class VMService : IVMService
    {
        public ApplicationDbContext _context;
        private readonly IDbService _dbService;

        public VMService(ApplicationDbContext context, IDbService dbService)
        {
            _context = context;
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
            IEnumerable<VirtualServer> virtualServers = _context.VirtualServers;
            string apiKey = GetvCenterApiKey().Result;
            string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm";
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("vmware-api-session-id", apiKey);
                using (var response = (httpClient.SendAsync(request)))
                {
                    string jsonResponse = response.Result.Content.ReadAsStringAsync().Result;//.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(jsonResponse);
                    IList<JToken> results = data["value"].Children().ToList();
                    int i = 1;
                    foreach (JToken result in results)
                    {
                        VirtualServer virtualServer = result.ToObject<VirtualServer>();

                        if (_dbService.IsVirtualStorageServer(virtualServer.Name) == false) //kontrola, zda se nejedná o virtuální storage server, který mezi tyto virtuální servery nepatří
                        {
                            if (result.Value<string>("power_state") == "POWERED_ON") virtualServer.StartServerOnStart = true;
                            VirtualServer virtualServerDB = virtualServers.FirstOrDefault(v => v.VMId == virtualServer.VMId);
                            if (virtualServerDB == null)
                            {
                                if (virtualServer.Name == "vcenter") virtualServer.IsvCenter = true;
                                else
                                {
                                    virtualServer.Order = i;
                                    i++;
                                }
                                _context.VirtualServers.Add(virtualServer);
                            }
                            else if (virtualServerDB.VMId == virtualServerDB.VMId && virtualServerDB.Name != virtualServer.Name)
                            {
                                virtualServerDB.Name = virtualServer.Name;
                                _context.Attach(virtualServerDB).State = EntityState.Modified;
                            }
                        }
                    }
                }
            }
            _context.SaveChanges();
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
            VirtualServer virtualServer = _context.VirtualServers.Include(i => i.Log).First(v => v.VMId == vmId);
            string apiKey = GetvCenterApiKey().Result;
            string apiUrl = _dbService.GetSettings().vCenterApiUrl + "/vcenter/vm/" + vmId + "/power/start";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Headers.Add("vmware-api-session-id", apiKey);
                var response = await httpClient.SendAsync(request);
                //virtualServer.Log.Add(new LogItem() { VirtualServerVMId = vmId, DateTime = DateTime.Now, OperationType = OperationTypeEnum.VMStart });
                //_context.SaveChanges();
                return response.StatusCode;
            }
        }
        public async Task<HttpStatusCode> ShutdownVirtualServer(string vmId)
        {
            VirtualServer virtualServer = _context.VirtualServers.Include(i => i.Log).First(v => v.VMId == vmId);
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
        Task<HttpStatusCode> ShutdownVirtualServer(string vmId);
        PowerStateEnum GetPowerVirtualServer(string vmId);
        PowerStateEnum GetvCenterState();
    }
}
