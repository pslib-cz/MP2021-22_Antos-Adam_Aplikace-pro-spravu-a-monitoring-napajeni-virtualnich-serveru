#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;
        private readonly IUPSService _upsService;

        public IndexModel(IDbService dbService, IServerService serverService, IUPSService upsService)
        {
            _dbService = dbService;
            _serverService = serverService;
            _upsService = upsService;
        }

        public AppSettings Settings { get; set; }
        public List<UPSLogItem> UPSLogItems { get; set; }
        public List<HostServer> HostServers { get; set; }
        public Dictionary<int, PowerStateEnum> HostServersPowerStates { get; set; }

        public void OnGet()
        {
            UPSLogItems = new List<UPSLogItem>();
            Settings = _dbService.GetSettings();
            List<Model.UPS> upss = _dbService.GetUPSs();
            foreach (Model.UPS ups in upss)
            {
                var logItem = _upsService.GetUPSLogItem(ups);
                logItem.UPS = ups;
                UPSLogItems.Add(logItem);
            }
            HostServers = _dbService.GetHostServers();
            HostServersPowerStates = new Dictionary<int, PowerStateEnum>();
            foreach (HostServer host in HostServers)
            {
                HostServersPowerStates.Add(host.Id, _serverService.GetHostServeriLOStatus(host).Result);
            }
        }
    }
}