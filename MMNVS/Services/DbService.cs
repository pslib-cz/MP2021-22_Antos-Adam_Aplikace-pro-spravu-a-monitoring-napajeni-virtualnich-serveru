#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Services
{
    public class DbService : IDbService
    {
        private readonly ApplicationDbContext _context;

        public DbService(ApplicationDbContext context)
        {
            _context = context;
        }

        public AppSettings GetSettings()
        {
            AppSettings settings;
            settings = _context.Settings.Include(a => a.PrimaryUPS).FirstOrDefault();

            if (settings == null)
            {
                settings = CreateSettings();
            }
            return settings;
        }

        public AppSettings GetSettingsWithoutInclude()
        {
            AppSettings settings;
            settings = _context.Settings.FirstOrDefault();

            if (settings == null)
            {
                settings = CreateSettings();
            }
            return settings;
        }

        private AppSettings CreateSettings()
        {
            AppSettings settings = new AppSettings() { DelayTime = 10, DelayTimeDatastores = 10, DelayTimeHosts = 20, DelayTimeVMStart = 20, MinBatteryTimeForShutdown = 1200, MinBatteryTimeForStart = 900, BatteryTimeForShutdownHosts = 250 };
            AddItem(settings);
            return settings;
        }

        public void UpdateSettings(AppSettings newAppSettings)
        {
            _context.Attach(newAppSettings).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<VirtualServer> GetVirtualServers()
        {
            return _context.VirtualServers.OrderBy(s => s.Order).ToList();
        }

        public List<VirtualServer> GetVirtualServersShutdown()
        {
            return _context.VirtualServers.OrderByDescending(s => s.Order).Where(v => v.IsvCenter == false).ToList();
        }
        public List<VirtualServer> GetVirtualServersStart()
        {
            return _context.VirtualServers.OrderBy(s => s.Order).Where(v => v.IsvCenter == false && v.StartServerOnStart == true).ToList();
        }
        public List<UPS> GetUPSs()
        {
            return _context.UPS.ToList();
        }
        public List<HostServer> GetHostServers()
        {
            return _context.HostServers.ToList();
        }

        public List<VirtualStorageServer> GetStorageServers(int hostId, bool includeDatastores = false)
        {
            if (includeDatastores == true) return _context.VirtualStorageServers.Include(d => d.Datastores).Where(s => s.HostId == hostId).ToList();
            return _context.VirtualStorageServers.Where(s => s.HostId == hostId).ToList();
        }

        public VirtualServer GetVirtualServer(string id)
        {
            return _context.VirtualServers.FirstOrDefault(v => v.VMId == id);
        }
        public VirtualServer GetVirtualServerByOrder(int? order)
        {
            return _context.VirtualServers.FirstOrDefault(v => v.Order == order);
        }

        public void EditItem (Object item)
        {
            _context.Attach(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public SystemStateEnum GetSystemState()
        {
            return GetSettingsWithoutInclude().SystemState;
        }

        public void SetSystemState(SystemStateEnum state)
        {
            AppSettings settings = GetSettingsWithoutInclude();
            settings.SystemState = state;
            UpdateSettings(settings);
            Log(new LogItem { OperationType = OperationTypeEnum.ChangeSystemState, DateTime = DateTime.Now, SystemState = state });
        }

        public List<MyUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUser(string username, string password, string notes = "")
        {
            var hasher = new PasswordHasher<MyUser>();
            MyUser user = new MyUser()
            {
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                PasswordHash = hasher.HashPassword(null, password),
                Notes = notes
            };
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void RemoveUser(string id)
        {
            MyUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public MyUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public List<Datastore> GetDatastores(int? storageServerId = null)
        {
            if (storageServerId == null)
            {
                return _context.Datastores.ToList();
            }
            else
            {
                return _context.Datastores.Where(d => d.VirtualStorageServerId == storageServerId).ToList();
            }
        }

        public void RemoveItem(object item)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }

        public VirtualStorageServer GetStorageServer(int? id)
        {
            return _context.VirtualStorageServers.FirstOrDefault(s => s.Id == id);
        }

        public HostServer GetHostServer(int? id)
        {
            return _context.HostServers.FirstOrDefault(h => h.Id == id);
        }

        public void AddItem(object item)
        {
            _context.Add(item);
            _context.SaveChanges();
        }

        public List<UPSLogItem> GetUPSLog(int count = 0, int start = 1)
        {
            if (count == 0) return _context.UPSLog.OrderByDescending(l => l.DateTime).ToList();
            int _start = start - 1;
            return _context.UPSLog.Include(u => u.UPS).OrderByDescending(l => l.DateTime).Skip(_start * count).Take(count).ToList();
        }

        public List<LogItem> GetLog(int count = 0, int start = 1)
        {
            if (count == 0) return _context.Log.OrderByDescending(l => l.DateTime).ToList();
            int _start = start - 1;
            return _context.Log.OrderByDescending(l => l.DateTime).Skip(_start * count).Take(count).ToList();
        }

        public bool IsVirtualStorageServer(string name)
        {
            List<VirtualStorageServer> virtualStorageServers = _context.VirtualStorageServers.ToList();
            foreach (var storageServer in virtualStorageServers)
            {
                if (name == storageServer.Name) return true;
            }
            return false;
        }
        public VirtualServer GetvCenter()
        {
            VirtualServer vCenter = _context.VirtualServers.FirstOrDefault(v => v.IsvCenter == true);
            return vCenter;
        }

        public void Log(LogItem logItem)
        {
            _context.Add(logItem);
            _context.SaveChanges();
        }
        public void LogUPS(UPSLogItem upsLogItem)
        {
            _context.Add(upsLogItem);
            _context.SaveChanges();
        }

        public int GetLogPagesCount(int count)
        {
            int itemsCount = _context.Log.Count();
            return (itemsCount / count) + 1;
        }
        public int GetUPSLogPagesCount(int count)
        {
            int itemsCount = _context.UPSLog.Count();
            return (itemsCount / count) + 1;
        }

        public void SetvCenter(VirtualServer newVCenter)
        {
            VirtualServer oldVCenter = GetvCenter();
            oldVCenter.Order = newVCenter.Order;
            newVCenter.Order = null;
            oldVCenter.IsvCenter = false;
            newVCenter.IsvCenter = true;
            EditItem(oldVCenter);
            EditItem(newVCenter);
        }

        public UPS GetUPS(int? id)
        {
            return _context.UPS.FirstOrDefault(u => u.Id == id);
        }

        public void AddVirtualServer(VirtualServer virtualServer, bool isvCenter = false)
        {
            if (isvCenter == false)
            {
                int count = _context.VirtualServers.Count();
                virtualServer.Order = count + 1;
            }
            else if (isvCenter == true) virtualServer.IsvCenter = true;
            AddItem(virtualServer);
        }

        public void RemoveVirtualServer(VirtualServer virtualServer)
        {
            List<VirtualServer> virtualServers = _context.VirtualServers.Where(v => v.Order >= virtualServer.Order).ToList();
            foreach (VirtualServer vm in virtualServers)
            {
                vm.Order--;
            }
            _context.SaveChanges();
            RemoveItem(virtualServer);
        }

        public void RemoveHostServer(int? id)
        {
            HostServer host = _context.HostServers.Include(h => h.VirtualStorageServers).ThenInclude(g => g.Datastores).FirstOrDefault(s => s.Id == id);

            if (host != null)
            {
                foreach (VirtualStorageServer storageServer in host.VirtualStorageServers)
                {
                    foreach (Datastore datastore in storageServer.Datastores)
                    {
                        RemoveItem(datastore);
                    }
                    RemoveItem(storageServer);
                }
                RemoveItem(host);
            }
        }

        public void RemoveStorageServer(int? id)
        {
            VirtualStorageServer storageServer = _context.VirtualStorageServers.Include(g => g.Datastores).FirstOrDefault(s => s.Id == id);
            if (storageServer != null)
            {
                foreach (Datastore datastore in storageServer.Datastores)
                {
                    RemoveItem(datastore);
                }
            RemoveItem(storageServer);
            }
        }

        public Datastore GetDatastore(int? id)
        {
            return _context.Datastores.FirstOrDefault(d => d.Id == id);
        }
    }

    public interface IDbService
    {
        AppSettings GetSettings();
        AppSettings GetSettingsWithoutInclude();
        void UpdateSettings(AppSettings newAppSettings);
        List<VirtualServer> GetVirtualServers();
        List<VirtualServer> GetVirtualServersShutdown();
        List<VirtualServer> GetVirtualServersStart();
        VirtualServer GetVirtualServer(string id);
        VirtualServer GetVirtualServerByOrder(int? order);
        List<UPS> GetUPSs();
        List<HostServer> GetHostServers();
        List<VirtualStorageServer> GetStorageServers(int hostId, bool includeDatastores = false);
        List<UPSLogItem> GetUPSLog(int count = 0, int start = 1);
        List<LogItem> GetLog(int count = 0, int start = 1);
        void AddItem(Object item);
        void EditItem(Object item);
        void RemoveItem(Object item);
        SystemStateEnum GetSystemState();
        void SetSystemState(SystemStateEnum state);
        List<MyUser> GetUsers();
        void AddUser(string username, string password, string notes = "");
        void RemoveUser(string id);
        MyUser GetUser(string id);
        List<Datastore> GetDatastores(int? storageServerId = null);
        VirtualStorageServer GetStorageServer(int? id);
        HostServer GetHostServer(int? id);
        bool IsVirtualStorageServer(string name);
        VirtualServer GetvCenter();
        void Log(LogItem logItem);
        void LogUPS(UPSLogItem upsLogItem);
        int GetLogPagesCount(int count = 1);
        int GetUPSLogPagesCount(int count = 1);
        void SetvCenter(VirtualServer newVCenter);
        UPS GetUPS(int? id);
        void AddVirtualServer(VirtualServer virtualServer, bool isvCenter = false);
        void RemoveVirtualServer(VirtualServer virtualServer);
        void RemoveHostServer(int? id);
        void RemoveStorageServer(int? id);
        Datastore GetDatastore(int? id);
    }
}