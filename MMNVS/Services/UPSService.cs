using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using MMNVS.Data;
using MMNVS.Model;
using System.Net;

namespace MMNVS.Services
{
    public class UPSService : IUPSService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbService _dbService;

        public UPSService(ApplicationDbContext context, IDbService dbService)
        {
            _context = context;
            _dbService = dbService;
        }

        public UPSLogItem GetUPSLogItem(UPS ups)
        {
            UPSLogItem logItem = new UPSLogItem() { UPSId = ups.Id, DateTime = DateTime.Now};
            try
            {
                var dataUps = Messenger.Get(VersionCode.V1,
                new IPEndPoint(IPAddress.Parse(ups.IPAddress), 161),
                new OctetString("public"),
                new List<Variable> { new Variable(new ObjectIdentifier(ups.SNMPOIDBatteryCapacity)), new Variable(new ObjectIdentifier(ups.SNMPOIDBatteryTime)), new Variable(new ObjectIdentifier(ups.SNMPOIDState)), new Variable(new ObjectIdentifier(ups.SNMPOIDLoad)) },
                1000);
                logItem.BatteryCapacity = Int32.Parse(dataUps[0].Data.ToString());
                logItem.RemainingTime = Int32.Parse(dataUps[1].Data.ToString());
                int upsState = Int32.Parse(dataUps[2].Data.ToString());
                if (upsState == 3) logItem.State = UPSStateEnum.MainSupply;
                else if (upsState == 2) logItem.State = UPSStateEnum.Battery;
                logItem.Load = Int32.Parse(dataUps[3].Data.ToString());
            }
            catch
            {
                logItem.Error = true;
            }
            //logItem.State = UPSStateEnum.Battery; //pouze pro testování aplikace
            return logItem;
        }

        public void LogUPSsData()
        {
            List<UPS> ups = _context.UPS.ToList();
            foreach (UPS u in ups)
            {
                _context.UPSLog.Add(GetUPSLogItem(u));
            }
            _context.SaveChanges();
        }
        public bool IsUPSsOnMainSuply()
        {
            List<UPS> ups = _dbService.GetUPSs();
            int errors = 0;
            foreach (UPS u in ups)
            {
                try
                {
                    if (GetUPSLogItem(u).State == UPSStateEnum.Battery) return false;
                }
                catch
                {
                    errors++;
                }
            }
            if (ups.Count > errors) return true; //pokud je dostupná alespoň 1 ups, která je připojena k el. síti
            else return false;
        }

        public int GetRemaingTimeAllUPSs()
        {
            List<UPS> ups = _dbService.GetUPSs();
            int time = 0;
            foreach (UPS u in ups)
            {
                try
                {
                    time += GetUPSLogItem(u).RemainingTime;
                }
                catch
                {
                }
            }
            return time / ups.Count();
        }
    }
    public interface IUPSService
    {
        void LogUPSsData();
        UPSLogItem GetUPSLogItem(UPS ups);
        bool IsUPSsOnMainSuply();
        int GetRemaingTimeAllUPSs();
    }
}
