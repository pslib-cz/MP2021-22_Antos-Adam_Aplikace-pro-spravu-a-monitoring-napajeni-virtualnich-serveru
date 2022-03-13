#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class EditModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;
        public int HostServerId { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public EditModel(IDbService dbService, IServerService serverService)
        {
            _dbService = dbService;
            _serverService = serverService;
        }

        [BindProperty]
        public VirtualStorageServer VirtualStorageServer { get; set; }

        public ActionResult OnGet(int? id, int hostServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostServerId = hostServerId;
            VirtualStorageServer = _dbService.GetStorageServer(id);

            if (VirtualStorageServer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost(int hostServerId)
        {
            _dbService.EditItem(VirtualStorageServer);

            if (_serverService.GetStorageServerStatus(VirtualStorageServer) == PowerStateEnum.PoweredOn)
            {
                SuccessMessage = "Připojení k serveru bylo úspěšné.";
            }

            else
            {
                ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
            }

            return Redirect("./Index?hostServerId=" + hostServerId);
        }
    }
}
