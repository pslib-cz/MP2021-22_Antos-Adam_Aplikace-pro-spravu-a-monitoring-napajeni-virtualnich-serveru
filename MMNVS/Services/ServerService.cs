using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.AspNetCore.Mvc;
using MMNVS.Data;
using MMNVS.Model;
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
            pipeline.Commands.AddScript("Import-Module VMware.VimAutomation.Core"/*"Get-Module -Name VMware* -ListAvailable | Import-Module"*/);
            pipeline.Commands.AddScript("Connect-VIServer -Server " + host.ESXiIPAddress + " -Protocol https -User " + host.ESXiUser + " -Password " + host.ESXiPassword + " -ErrorAction Stop");
            pipeline.Commands.AddScript("Get-VM -Name " + storageServer.Name + " -Server " + host.ESXiIPAddress + " | Start-VM");
            pipeline.Invoke();
            runspace.Close();
        }
    }
    public interface IServerService
    {
        void ShutdownStorageServer(VirtualStorageServer storageServer);
        void ShutdownHost(HostServer host);
        PowerStateEnum GetStorageServerStatus(VirtualStorageServer storageServer);
        void/*ActionResult*/ ServerDataStoreCheck(int storageServerId);
        void/*ActionResult*/ StorageRescan(HostServer host);
        void CheckStartvCenterHost(HostServer host);
        void CheckShutdownvCenterHost(HostServer host);
        HostServer FindStartvCenter();
        HostServer FindShutdownvCenter();
        PowerStateEnum GetHostServerStatus(HostServer host);
        void StartStorageServer(VirtualStorageServer storageServer);
        bool DataStoreCheck(Datastore datastore);
    }
}
