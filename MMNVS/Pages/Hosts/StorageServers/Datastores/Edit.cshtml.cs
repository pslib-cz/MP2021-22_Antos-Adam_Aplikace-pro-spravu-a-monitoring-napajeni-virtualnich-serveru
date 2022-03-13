#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class EditModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;
        public int StorageServerId { get; set; }

        public EditModel(IDbService dbService, IServerService serverService)
        {
            _dbService = dbService;
            _serverService = serverService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public Datastore Datastore { get; set; }

        public ActionResult OnGet(int? id, int storageServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Datastore = _dbService.GetDatastore(id);
            StorageServerId = storageServerId;

            if (Datastore == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost(int storageServerId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbService.EditItem(Datastore);

            if (_serverService.DataStoreCheck(Datastore) == true) SuccessMessage = "Připojení k datastoru bylo úspěšné.";
            else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";

            return Redirect("./Index?storageServerId=" + storageServerId);
        }
    }
}
