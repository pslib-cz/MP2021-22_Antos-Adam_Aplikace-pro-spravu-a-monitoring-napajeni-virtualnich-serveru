﻿using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbService _dbService;
        private readonly IUPSService _upsservice;
        private readonly IVMService _vmService;
        private readonly IServerService _serverService;
        private readonly IMailService _mailService;

        public ScenarioService(ApplicationDbContext context, IDbService dbService, IUPSService upsservice, IVMService vmService, IServerService serverService, IMailService mailService)
        {
            _context = context;
            _dbService = dbService;
            _upsservice = upsservice;
            _vmService = vmService;
            _serverService = serverService;
            _mailService = mailService;
        }

        public void CheckUps()
        {
            _upsservice.LogUPSsData();
            bool isUPSOnMainSuply = _upsservice.IsUPSsOnMainSuply();
            SystemStateEnum systemState = _dbService.GetSystemState();
            if (isUPSOnMainSuply == true && systemState == SystemStateEnum.Off) //USP připojena(y) k síti a servery vypnuté
            {
                throw new NotImplementedException();
                if (_upsservice.GetRemaingTimeAllUPSs() >= _dbService.GetSettingsWithoutInclude().MinBatteryTimeForStart) //spouštění pouze při na nabitých bateriích
                {
                    Start();
                }
            }
            else if (isUPSOnMainSuply == false && systemState == SystemStateEnum.Running) //UPS na bateriích a servery spuštěné
            {
                throw new NotImplementedException();
                Shutdown();
            }
            else
            {
                //throw new NotImplementedException();//pouze pro testovani
            }
        }

        public void Shutdown()
        {
            if (_dbService.GetSystemState() == SystemStateEnum.Off || _dbService.GetSystemState() == SystemStateEnum.Startup || _dbService.GetSystemState() == SystemStateEnum.Shutdown) return;
            if (_dbService.GetSystemState() != SystemStateEnum.Interrupt) _mailService.SendMail("Začátek vypínání", "Servery se začaly vypínat.");
            _dbService.SetSystemState(SystemStateEnum.Shutdown);

            //Vypínání virtuálních serverů a nakonec i vCentru (nejvyšší order)
            List<VirtualServer> virtualServers = _dbService.GetVirtualServers();
            List<HostServer> hosts = _dbService.GetHostServers();
            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VMsShutdownBegin, DateTime = DateTime.Now });
            foreach (var virtualServer in virtualServers)
            {
                if (_upsservice.IsUPSsOnMainSuply() == false) //true - připojeno k el. síti, false - baterie
                {
                    if (_vmService.GetPowerVirtualServer(virtualServer.VMId) == PowerStateEnum.PoweredOn)
                    {
                        _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VMShutdownBegin, DateTime = DateTime.Now, VirtualServerVMId = virtualServer.VMId });
                        _vmService.ShutdownVirtualServer(virtualServer.VMId);
                        while (_vmService.GetPowerVirtualServer(virtualServer.VMId) == PowerStateEnum.PoweredOn)
                        {
                            Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
                        }
                        _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VMShutdownEnd, DateTime = DateTime.Now, VirtualServerVMId = virtualServer.VMId });
                    }
                }
                else
                {
                    _dbService.SetSystemState(SystemStateEnum.Interrupt);
                    Start(); //asi
                    //obnovila se dodávka elektřiny - asi znovu start vypnutých vm
                    return; //ukončení provádění vypínání
                }
            }


            //vypnutí vCenter
            VirtualServer vCenter = _dbService.GetvCenter();
            if (_vmService.GetPowerVirtualServer(vCenter.VMId) == PowerStateEnum.PoweredOn)
            {
                _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.vCenterShutdownBegin, DateTime = DateTime.Now });
                _vmService.ShutdownVirtualServer(vCenter.VMId);
                while (_vmService.GetPowerVirtualServer(vCenter.VMId) == PowerStateEnum.PoweredOn)
                {
                    Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
                }
                _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.vCenterShutdownEnd, DateTime = DateTime.Now });
            }


            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VMsOff, DateTime = DateTime.Now });
            //Vypínání VSA (storage) serverů
            List<VirtualStorageServer> storageServers = _context.VirtualStorageServers.ToList();
            foreach (var storageServer in storageServers)
            {
                if (_upsservice.IsUPSsOnMainSuply() == false) //true - připojeno k el. síti, false - baterie
                {
                    _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VSAShutdownBegin, DateTime = DateTime.Now, VirtualStorageServerId = storageServer.Id });
                    _serverService.ShutdownStorageServer(storageServer);
                    while (_serverService.GetStorageServerStatus(storageServer) == PowerStateEnum.PoweredOn)
                    {
                        Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
                    }
                    _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VSAShutdownEnd, DateTime = DateTime.Now, VirtualStorageServerId = storageServer.Id });
                }
                else
                {
                    _dbService.SetSystemState(SystemStateEnum.Interrupt);
                    Start(); //asi
                }
            }

            //Vypínání host serverů
            //Kontrola kapacity UPS, opožděné vypínání
            foreach (var host in hosts)
            {
                if (_upsservice.IsUPSsOnMainSuply() == false) //true - připojeno k el. síti, false - baterie
                {
                    if (_serverService.GetHostServerStatus(host) == PowerStateEnum.PoweredOn)
                    {
                        _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VSAShutdownBegin, DateTime = DateTime.Now, HostServerId = host.Id });
                        _serverService.ShutdownHost(host);
                        while (_serverService.GetHostServerStatus(host) == PowerStateEnum.PoweredOn)
                        {
                            Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
                        }
                        _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VSAShutdownEnd, DateTime = DateTime.Now, HostServerId = host.Id });
                    }
                }
                else
                {
                    _dbService.SetSystemState(SystemStateEnum.Interrupt);
                    Start(); //asi
                }
            }

            _mailService.SendMail("Servery vypnuty", "Servery byly vypnuty.");
            _dbService.SetSystemState(SystemStateEnum.Off);

        }

        public void Start()
        {
            if (_dbService.GetSystemState() == SystemStateEnum.Startup || _dbService.GetSystemState() == SystemStateEnum.Shutdown) return;
            if (_dbService.GetSystemState() != SystemStateEnum.Interrupt) _mailService.SendMail("Začátek spouštění", "Servery se začaly spouštět.");
            _dbService.SetSystemState(SystemStateEnum.Startup);
            List<HostServer> hosts = _dbService.GetHostServers();
            //Kontrola host serverů a případné spouštění storage serverů
            foreach (var host in hosts)
            {
                if (_upsservice.IsUPSsOnMainSuply() == true)
                {
                    if (_serverService.GetHostServerStatus(host) != PowerStateEnum.PoweredOn)
                    {
                        _mailService.SendMail("Server " + host.Name + " je vypnut!", "Server " + host.Name + " je vypnut!");
                        _dbService.SetSystemState(SystemStateEnum.Off);
                        return;
                        //zapnutí host serveru
                        //čekání
                    }
                    List<VirtualStorageServer> storageServers = _dbService.GetStorageServers(host.Id);
                    foreach (var storageServer in storageServers)
                    {
                        if (_upsservice.IsUPSsOnMainSuply() == true) //true - připojeno k el. síti, false - baterie
                        {
                            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VSAStartupBegin, DateTime = DateTime.Now, VirtualStorageServerId = storageServer.Id, HostServerId = host.Id });
                            _serverService.StartStorageServer(storageServer);
                            while (_serverService.GetStorageServerStatus(storageServer) == PowerStateEnum.PoweredOff)
                            {
                                Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
                            }
                            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VSAStartupEnd, DateTime = DateTime.Now, VirtualStorageServerId = storageServer.Id, HostServerId = host.Id });
                        }
                        else
                        {
                            _dbService.SetSystemState(SystemStateEnum.Interrupt);
                            Shutdown(); //asi
                        }
                    }
                }
                else
                {
                    _dbService.SetSystemState(SystemStateEnum.Interrupt);
                    Shutdown();
                }
            }
            ////Rescan datastorů

            ////asi čekání
            //foreach (var host in hosts)
            //{
            //    _serverService.StorageRescan(host);
            //}

            //Kontrola připojení datastorů
            List<Datastore> datastores = _dbService.GetDatastores();
            foreach (Datastore datastore in datastores)
            {
                VirtualStorageServer storageServer = _dbService.GetStorageServer(datastore.VirtualStorageServerId);
                HostServer host = _dbService.GetHostServer(storageServer.HostId);
                _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.DatastoreCheckBegin, DateTime = DateTime.Now, VirtualStorageServerId = storageServer.Id, HostServerId = host.Id, DatastoreId = datastore.Id });
                while (_serverService.DataStoreCheck(datastore) == false)
                {
                    Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
                    /*if (_serverService.GetHostServerStatus(host) == PowerStateEnum.PoweredOn) */_serverService.StorageRescan(host);
                }
                _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.DatastoreCheckEnd, DateTime = DateTime.Now, VirtualStorageServerId = storageServer.Id, HostServerId = host.Id, DatastoreId = datastore.Id });
            }

            //Spouštění vCenter
            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.vCenterStartBegin, DateTime = DateTime.Now });
            _serverService.FindStartvCenter();

            //Kontrola spuštění vCenter
            while(_vmService.GetvCenterState() != PowerStateEnum.PoweredOn)
            {
                Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //x sekund čeká, poté kontrola znovu
            }
            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.vCenterStartEnd, DateTime = DateTime.Now });


            //Spouštění virtuálních serverů
            List<VirtualServer> virtualServers = _dbService.GetVirtualServers();
            foreach (var virtualServer in virtualServers)
            {
                if (_upsservice.IsUPSsOnMainSuply()) //true - připojeno k el. síti, false - baterie
                {
                    if (_vmService.GetPowerVirtualServer(virtualServer.VMId) != PowerStateEnum.PoweredOn)
                    {
                        _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VMStartBegin, DateTime = DateTime.Now, VirtualServerVMId = virtualServer.VMId });
                        _vmService.StartVirtualServer(virtualServer.VMId);
                        Thread.Sleep(_dbService.GetSettingsWithoutInclude().DelayTime * 1000); //pauza pro start Vm !!! zmenit
                        _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.VMStartEnd, DateTime = DateTime.Now, VirtualServerVMId = virtualServer.VMId });
                    }

                }
                else
                {
                    _dbService.SetSystemState(SystemStateEnum.Interrupt);
                    Shutdown(); //asi
                    //opět byla přerušena dodávka elektřiny
                    return; //ukončení provádění spouštění
                }
            }

            _mailService.SendMail("Servery spuštěny", "Servery byly spuštěny.");
            _dbService.SetSystemState(SystemStateEnum.Running);
        }
    }
    public interface IScenarioService
    {
        void Start();
        void Shutdown();
        void CheckUps();
    }
}
