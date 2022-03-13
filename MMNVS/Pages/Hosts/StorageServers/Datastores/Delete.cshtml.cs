#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class DeleteModel : PageModel
    {
        private readonly IDbService _dbService;
        public int StorageServerId { get; set; }

        public DeleteModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Datastore Datastore { get; set; }

        public ActionResult OnGet(int? id, int storageServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            StorageServerId = storageServerId;
            Datastore = _dbService.GetDatastore(id);

            if (Datastore == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost(int? id, int storageServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Datastore = _dbService.GetDatastore(id);

            if (Datastore != null)
            {
                _dbService.RemoveItem(Datastore);
            }

            return Redirect("./Index?storageServerId=" + storageServerId);
        }
    }
}
