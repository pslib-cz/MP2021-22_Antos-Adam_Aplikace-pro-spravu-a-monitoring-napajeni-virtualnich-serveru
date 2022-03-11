using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.AspNetCore.Mvc;
using MMNVS.Data;
using MMNVS.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Text;

namespace MMNVS.Services
{
    public class ServerService : IServerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbService _dbService;

        public ServerService(ApplicationDbContext context, IDbService dbService)
        {
            _context = context;
            _dbService = dbService;
        }
        public void/*ActionResult*/ ServerDataStoreCheck(int storageServerId)
        {
            VirtualStorageServer storageServer = _context.VirtualStorageServers.FirstOrDefault(s => s.Id == storageServerId);
            if (storageServer != null)
            {
                foreach (Datastore datastore in storageServer.Datastores)
                {
                    DataStoreCheck(datastore);
                }
            }

        }
        public bool DataStoreCheck(Datastore datastore)
        {
            string vCenterName = _dbService.GetSettingsWithoutInclude().vCenterIP;
            VirtualStorageServer storageServer = _context.VirtualStorageServers.FirstOrDefault(s => s.Id == datastore.VirtualStorageServerId);
            HostServer host = _context.HostServers.FirstOrDefault(h => h.Id == storageServer.HostId);
            //kontrola dostupnosti datastoru
            try
            {
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();

                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
                pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
                pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
                pipeline.Commands.AddScript("Get-Datastore -Name " + datastore.Name + " -ErrorAction Stop");
                pipeline.Invoke();
                runspace.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void/*ActionResult*/ StorageRescan(HostServer host) //rescan seznamu virtuálních datastorů (po spuštění VSA serverů)
        {
            string vCenterName = _dbService.GetSettingsWithoutInclude().vCenterIP;
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VMHostStorage -RescanAllHba");
            pipeline.Invoke();
            runspace.Close();
        }

        public HostServer FindStartvCenter()
        {
            List<HostServer> hostServers = _context.HostServers.ToList();
            foreach (HostServer hostServer in hostServers)
            {
                try
                {
                    CheckStartvCenterHost(hostServer);
                    return hostServer;
                }
                catch
                {

                }
            }
            return null;
        }
        public void CheckStartvCenterHost(HostServer host)
        {
            string vCenterName = _dbService.GetvCenter().Name;
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User "+ host.ESXiUser +" -Password "+ host.ESXiPassword +" -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VM -Name "+ vCenterName + " -Server " + host.ESXiIPAddress + " | Start-VM" + " -ErrorAction Stop");
            pipeline.Invoke();
            runspace.Close();
        }

        public HostServer FindShutdownvCenter()
        {
            List<HostServer> hostServers = _context.HostServers.ToList();
            foreach (HostServer hostServer in hostServers)
            {
                try
                {
                    CheckShutdownvCenterHost(hostServer);
                    return hostServer;
                }
                catch
                {

                }
            }
            return null;
        }
        public void CheckShutdownvCenterHost(HostServer host)
        {
            string vCenterName = _dbService.GetvCenter().Name;
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VM -Name " + vCenterName + " -Server " + host.ESXiIPAddress + " | Shutdown-VMGuest -Confirm:$false");
            pipeline.Invoke();
            runspace.Close();
        }

        public void ShutdownStorageServer(VirtualStorageServer storageServer)
        {
            HostServer host = _context.HostServers.FirstOrDefault(h => h.Id == storageServer.HostId);
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VM -Name " + storageServer.Name + " | Shutdown-VMGuest -Confirm:$false");
            pipeline.Invoke();
            runspace.Close();
        }

        public void ShutdownHost(HostServer host)
        {
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Stop-VMHost -VMHost " + host.ESXiIPAddress + " -Force -Confirm:$false");
            pipeline.Invoke();
            runspace.Close();
        }

        public PowerStateEnum GetStorageServerStatus(VirtualStorageServer storageServer)
        {
            try
            {
                string vCenterName = _dbService.GetSettingsWithoutInclude().vCenterIP;
                HostServer host = _context.HostServers.FirstOrDefault(h => h.Id == storageServer.HostId);
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();

                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
                pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
                pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
                pipeline.Commands.AddScript("(Get-VM -Name \"" + storageServer.Name + "\").Powerstate");
                pipeline.Commands.Add("Out-string");
                Collection<PSObject> results = pipeline.Invoke();
                runspace.Close();
                string ps = results[0].ToString();
                if (ps == "PoweredOn\r\n") return PowerStateEnum.PoweredOn;
                else if (ps == "PoweredOff\r\n") return PowerStateEnum.PoweredOff;
                else return PowerStateEnum.Unknown;
            }
            catch
            {
                return PowerStateEnum.Unknown;
            }
        }

        public PowerStateEnum GetHostServerStatus(HostServer host)
        {
            try
            {
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();

                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
                pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
                pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop"); pipeline.Invoke();
                runspace.Close();
                return PowerStateEnum.PoweredOn;
            }
            catch
            {
                return PowerStateEnum.Unknown;
            }

        }

        public void StartStorageServer(VirtualStorageServer storageServer)
        {
            HostServer host = _context.HostServers.FirstOrDefault(h => h.Id == storageServer.HostId);
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy Unrestricted");
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core");
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VM -Name " + storageServer.Name + " -Server " + host.ESXiIPAddress + " | Start-VM");
            pipeline.Invoke();
            runspace.Close();
        }

        public async void StartHost(HostServer host)
        {
            string url = "https://" + host.iLoIPAddress + "/redfish/v1/Systems/1/Actions/ComputerSystem.Reset"; //Oem/Hpe/HpeComputerSystemExt.PowerButton/
            string authtoken = GetiLOApiKey(host);

            var myData = new
            {
                Action = "PowerButton",
                PushType = "Press",
                Target = "/Oem/Hp"
            };

            string jsonData = JsonConvert.SerializeObject(myData);

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);

            using (HttpClient httpClient = new HttpClient(handler))
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                request.Headers.Add("X-Auth-Token", authtoken);
                request.Headers.Add("OData-Version", "4.0");
                var response = await httpClient.SendAsync(request);
            }
        }

        public async Task<PowerStateEnum> GetHostServeriLOStatus(HostServer host)
        {
            try
            {
                string authtoken = GetiLOApiKey(host);
                string url = "https://" + host.iLoIPAddress + "/redfish/v1/Systems/1/";
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var client = new HttpClient(handler);
                using (var httpClient = new HttpClient(handler))
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-Auth-Token", authtoken);
                    using (var response = (await httpClient.SendAsync(request)))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(apiResponse);
                        var jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        string value = jsonResponse.SelectToken("$.PowerState").Value<string>();
                        if (value == "On") return PowerStateEnum.PoweredOn;
                        else if (value == "Off") return PowerStateEnum.PoweredOff;
                        else return PowerStateEnum.Unknown;
                    }
                }
            }
            catch
            {
                return PowerStateEnum.Unknown;
            }
        }

        public string GetiLOApiKey(HostServer host)
        {
            string url = "https://" + host.iLoIPAddress + "/redfish/v1/sessions/";
            string username = host.iLoUser;
            string password = host.iLoPassword;

            var myData = new
            {
                UserName = username,
                Password = password,
            };

            string jsonData = JsonConvert.SerializeObject(myData);

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);

            using (HttpClient httpClient = new HttpClient(handler))
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = httpClient.SendAsync(request);
                IEnumerable<string> cookieHeader;
                response.Result.Headers.TryGetValues("X-Auth-Token", out cookieHeader);
                string token = cookieHeader.ToArray()[0];
                return token;
            }
        }
    }
    public interface IServerService
    {
        void ShutdownStorageServer(VirtualStorageServer storageServer);
        void ShutdownHost(HostServer host);
        void StartHost(HostServer host);
        PowerStateEnum GetStorageServerStatus(VirtualStorageServer storageServer);
        void ServerDataStoreCheck(int storageServerId);
        void StorageRescan(HostServer host);
        void CheckStartvCenterHost(HostServer host);
        void CheckShutdownvCenterHost(HostServer host);
        HostServer FindStartvCenter();
        HostServer FindShutdownvCenter();
        PowerStateEnum GetHostServerStatus(HostServer host);
        Task<PowerStateEnum> GetHostServeriLOStatus(HostServer host);
        void StartStorageServer(VirtualStorageServer storageServer);
        bool DataStoreCheck(Datastore datastore);
        string GetiLOApiKey(HostServer host);
    }
}
