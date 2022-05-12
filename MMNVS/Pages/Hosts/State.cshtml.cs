#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Services;
using MMNVS.Model;

namespace MMNVS.Pages.Hosts
{
    public class StateModel : PageModel
    {
        private readonly IServerService _serverService;
        private readonly IDbService _dbService;

        public StateModel(IServerService serverService, IDbService dbService)
        {
            _serverService = serverService;
            _dbService = dbService;
        }

        public HostServer HostServer { get; set; }
        public PowerStateEnum iLOState { get; set; }
        public PowerStateEnum vSphereState { get; set; }
        public List<VirtualStorageServer> StorageServers { get; set; }
        public Dictionary<int, PowerStateEnum> StorageServersState { get; set; }
        public Dictionary<int, bool> DatastoresState { get; set; }

        public ActionResult OnGet(int id)
        {
            HostServer = _dbService.GetHostServer(id);
            if (HostServer == null) return NotFound();
            iLOState = _serverService.GetHostServeriLOStatus(HostServer).Result;
            if (HostServer.IsOSWindows == false)
            {
                vSphereState = _serverService.GetHostServerStatus(HostServer);
                StorageServers = _dbService.GetStorageServers(HostServer.Id, true);
                StorageServersState = new Dictionary<int, PowerStateEnum>();
                DatastoresState = new Dictionary<int, bool>();
                foreach (var storageServer in StorageServers)
                {
                    StorageServersState.Add(storageServer.Id, _serverService.GetStorageServerStatus(storageServer));
                    foreach (var datastore in storageServer.Datastores)
                    {
                        DatastoresState.Add(datastore.Id, _serverService.DataStoreCheck(datastore));
                    }
                }
            }
            return Page();
        }
    }
}