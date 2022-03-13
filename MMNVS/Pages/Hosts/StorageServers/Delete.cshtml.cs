#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class DeleteModel : PageModel
    {
        private readonly IDbService _dbService;

        public DeleteModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public int HostServerId { get; set; }

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

        public ActionResult OnPost(int? id, int hostServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            _dbService.RemoveStorageServer(id);

            return Redirect("./Index?hostServerId=" + hostServerId);
        }
    }
}
