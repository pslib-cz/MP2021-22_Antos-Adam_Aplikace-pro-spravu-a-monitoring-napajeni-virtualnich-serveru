#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class CreateModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;

        public CreateModel(IDbService dbService, IServerService serverService)
        {
            _dbService = dbService;
            _serverService = serverService;
        }

        public HostServer HostServer { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public VirtualStorageServer VirtualStorageServer { get; set; }
        public ActionResult OnGet(int hostServerId)
        {
            HostServer = _dbService.GetHostServer(hostServerId);
            return Page();
        }

        public ActionResult OnPost(int hostServerId)
        {
            HostServer = _dbService.GetHostServer(hostServerId);
            VirtualStorageServer.HostId = HostServer.Id;
            _dbService.AddItem(VirtualStorageServer);

            if (_serverService.GetStorageServerStatus(VirtualStorageServer) == PowerStateEnum.PoweredOn)
            {
                SuccessMessage = "Připojení k serveru bylo úspěšné.";
                return Redirect("./Datastores/Create?storageServerId=" + VirtualStorageServer.Id.ToString());
            }

            else
            {
                ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
                return Redirect("./Index?hostServerId=" + HostServer.Id.ToString());
            }
        }
    }
}
