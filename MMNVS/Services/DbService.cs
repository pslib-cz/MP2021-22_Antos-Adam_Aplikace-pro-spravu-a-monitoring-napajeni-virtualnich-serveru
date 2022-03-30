#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;
using System.Globalization;

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
            return GetSettings().SystemState;
        }

        public void SetSystemState(SystemStateEnum state)
        {
            AppSettings settings = GetSettings();
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

        public List<UPSLogItem> GetUPSLog(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 0, int start = 1)
        {
            if (count == 0) return _context.UPSLog.OrderByDescending(l => l.DateTime).Where(u => u.DateTime >= dateTimeFrom && u.DateTime <= dateTimeTo).ToList();
            int _start = start - 1;
            return _context.UPSLog.Include(u => u.UPS).OrderByDescending(l => l.DateTime).Where(u => u.DateTime >= dateTimeFrom && u.DateTime <= dateTimeTo).Skip(_start * count).Take(count).ToList();
        }

        public List<LogItem> GetLog(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 0, int start = 1)
        {
            if (count == 0) return _context.Log.OrderByDescending(l => l.DateTime).Where(u => u.DateTime >= dateTimeFrom && u.DateTime <= dateTimeTo).ToList();
            int _start = start - 1;
            return _context.Log.OrderByDescending(l => l.DateTime).Where(u => u.DateTime >= dateTimeFrom && u.DateTime <= dateTimeTo).Skip(_start * count).Take(count).ToList();
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

        public int GetLogPagesCount(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 1)
        {
            int itemsCount = _context.Log.Where(u => u.DateTime >= dateTimeFrom && u.DateTime <= dateTimeTo).Count();
            if (count <= 0) return 1;
            return (itemsCount / count) + 1;
        }
        public int GetUPSLogPagesCount(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 1)
        {
            int itemsCount = _context.UPSLog.Where(u => u.DateTime >= dateTimeFrom && u.DateTime <= dateTimeTo).Count();
            if (count <= 0) return 1;
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
                int count = _context.VirtualServers.Where(v => v.IsvCenter == false).Count();
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
            List<LogItem> log = _context.Log.Where(l => l.VirtualServerVMId == virtualServer.VMId).ToList();
            foreach (LogItem logItem in log)
            {
                RemoveItem(logItem);
            }
            RemoveItem(virtualServer);
        }

        public void RemoveHostServer(int? id)
        {
            HostServer host = _context.HostServers.Include(h => h.VirtualStorageServers).ThenInclude(g => g.Datastores).FirstOrDefault(s => s.Id == id);

            if (host != null)
            {
                foreach (VirtualStorageServer storageServer in host.VirtualStorageServers)
                {
                    RemoveStorageServer(storageServer.Id);
                }

                List<LogItem> log = _context.Log.Where(l => l.HostServerId == host.Id).ToList();
                foreach (LogItem logItem in log)
                {
                    RemoveItem(logItem);
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
                    RemoveDatastore(datastore.Id);
                }

                List<LogItem> log = _context.Log.Where(l => l.VirtualStorageServerId == storageServer.Id).ToList();
                foreach (LogItem logItem in log)
                {
                    RemoveItem(logItem);
                }

                RemoveItem(storageServer);
            }
        }

        public void RemoveDatastore(int? id)
        {
            Datastore datastore = _context.Datastores.FirstOrDefault(d => d.Id == id);
            if (datastore != null)
            {
                List<LogItem> log = _context.Log.Where(l => l.DatastoreId == datastore.Id).ToList();
                foreach (LogItem logItem in log)
                {
                    RemoveItem(logItem);
                }

                RemoveItem(datastore);
            }
        }

        public void RemoveUPS(int? id)
        {
            UPS ups = _context.UPS.FirstOrDefault(u => u.Id == id);
            if (ups != null)
            {
                List<UPSLogItem> upsLog = _context.UPSLog.Where(l => l.UPSId == ups.Id).ToList();
                foreach (UPSLogItem upsLogItem in upsLog)
                {
                    RemoveItem(upsLogItem);
                }

                RemoveItem(ups);
            }
        }

        public Datastore GetDatastore(int? id)
        {
            return _context.Datastores.FirstOrDefault(d => d.Id == id);
        }

        public DateTimeFromTo CheckDateNotNull(string dateTimeToStr, string dateTimeFromStr)
        {
            DateTime? dateTimeTo = null;
            DateTime? dateTimeFrom = null;

            try
            {
                dateTimeFrom = DateTime.ParseExact(dateTimeFromStr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                dateTimeTo = DateTime.ParseExact(dateTimeToStr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            catch { }

            DateTimeFromTo dateTimeFromTo = new DateTimeFromTo();

            if (dateTimeTo != null) dateTimeFromTo.DateTimeTo = (DateTime)dateTimeTo;
            else dateTimeFromTo.DateTimeTo = DateTime.Now;
            if (dateTimeFrom != null) dateTimeFromTo.DateTimeFrom = (DateTime)dateTimeFrom;
            else
            {
                dateTimeFromTo.DateTimeFrom = DateTime.Now;
                dateTimeFromTo.DateTimeFrom = dateTimeFromTo.DateTimeFrom.AddHours(-4);
            }
            dateTimeFromTo.DateTimeFrom = dateTimeFromTo.DateTimeFrom.AddMilliseconds(-dateTimeFromTo.DateTimeFrom.Millisecond);
            dateTimeFromTo.DateTimeTo = dateTimeFromTo.DateTimeTo.AddMilliseconds(-dateTimeFromTo.DateTimeTo.Millisecond);

            return dateTimeFromTo;
        }
    }

    public interface IDbService
    {
        AppSettings GetSettings();
        void UpdateSettings(AppSettings newAppSettings);
        List<VirtualServer> GetVirtualServers();
        List<VirtualServer> GetVirtualServersShutdown();
        List<VirtualServer> GetVirtualServersStart();
        VirtualServer GetVirtualServer(string id);
        VirtualServer GetVirtualServerByOrder(int? order);
        List<UPS> GetUPSs();
        List<HostServer> GetHostServers();
        List<VirtualStorageServer> GetStorageServers(int hostId, bool includeDatastores = false);
        List<UPSLogItem> GetUPSLog(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 0, int start = 1);
        List<LogItem> GetLog(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 0, int start = 1);
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
        int GetLogPagesCount(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 1);
        int GetUPSLogPagesCount(DateTime dateTimeTo, DateTime dateTimeFrom, int count = 1);
        void SetvCenter(VirtualServer newVCenter);
        UPS GetUPS(int? id);
        void AddVirtualServer(VirtualServer virtualServer, bool isvCenter = false);
        void RemoveVirtualServer(VirtualServer virtualServer);
        void RemoveHostServer(int? id);
        void RemoveStorageServer(int? id);
        void RemoveDatastore(int? id);
        void RemoveUPS(int? id);
        Datastore GetDatastore(int? id);
        DateTimeFromTo CheckDateNotNull(string dateTimeToStr, string dateTimeFromStr);
    }

    public class DateTimeFromTo
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
    }

}