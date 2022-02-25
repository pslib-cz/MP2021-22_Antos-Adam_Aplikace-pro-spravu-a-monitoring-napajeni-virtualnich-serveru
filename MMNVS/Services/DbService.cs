﻿using Microsoft.AspNetCore.Identity;
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
                settings = new AppSettings();
                _context.Settings.Add(settings);
                _context.SaveChanges();
            }
            return settings;
        }

        public AppSettings GetSettingsWithoutInclude()
        {
            AppSettings settings;
            settings = _context.Settings.FirstOrDefault();

            if (settings == null)
            {
                settings = new AppSettings();
                _context.Settings.Add(settings);
                _context.SaveChanges();
            }
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
        public List<UPS> GetUPSs()
        {
            return _context.UPS.ToList();
        }
        public List<HostServer> GetHostServers()
        {
            return _context.HostServers.ToList();
        }

        public List<VirtualStorageServer> GetStorageServers(int hostId)
        {
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
            _context.Log.Add(new LogItem { OperationType = OperationTypeEnum.ChangeSystemState, DateTime = DateTime.Now, SystemState = state });
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

        public List<Datastore> GetDatastores()
        {
            return _context.Datastores.ToList();
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

        public List<UPSLogItem> GetUPSLog()
        {
            return _context.UPSLog.Include(u => u.UPS).OrderByDescending(u => u.DateTime).ToList();
        }

        public List<LogItem> GetLog()
        {
            return _context.Log.OrderBy(l => l.DateTime).ToList();
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
}

    public interface IDbService
    {
        AppSettings GetSettings();
        AppSettings GetSettingsWithoutInclude();
        void UpdateSettings(AppSettings newAppSettings);
        List<VirtualServer> GetVirtualServers();
        VirtualServer GetVirtualServer(string id);
        VirtualServer GetVirtualServerByOrder(int? order);
        List<UPS> GetUPSs();
        List<HostServer> GetHostServers();
        List<VirtualStorageServer> GetStorageServers(int hostId);
        List<UPSLogItem> GetUPSLog();
        List<LogItem> GetLog();
        void AddItem(Object item);
        void EditItem(Object item);
        void RemoveItem(Object item);
        SystemStateEnum GetSystemState();
        void SetSystemState(SystemStateEnum state);
        List<MyUser> GetUsers();
        void AddUser(string username, string password, string notes = "");
        void RemoveUser(string id);
        MyUser GetUser(string id);
        List<Datastore> GetDatastores();
        VirtualStorageServer GetStorageServer(int? id);
        HostServer GetHostServer(int? id);
        bool IsVirtualStorageServer(string name);
        VirtualServer GetvCenter();
    }
}