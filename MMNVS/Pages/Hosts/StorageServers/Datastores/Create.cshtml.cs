#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
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
        public VirtualStorageServer StorageServer { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public Datastore Datastore { get; set; }

        public ActionResult OnGet(int storageServerId)
        {
            StorageServer = _dbService.GetStorageServer(storageServerId);
            return Page();
        }

        public ActionResult OnPost(int storageServerId)
        {
            StorageServer = _dbService.GetStorageServer(storageServerId);
            Datastore.VirtualStorageServerId = StorageServer.Id;
            _dbService.AddItem(Datastore);
            if (_serverService.DataStoreCheck(Datastore) == true) SuccessMessage = "Připojení k datastoru bylo úspěšné.";
            else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
            return Redirect("./Index?storageServerId=" + storageServerId);
        }
    }
}
