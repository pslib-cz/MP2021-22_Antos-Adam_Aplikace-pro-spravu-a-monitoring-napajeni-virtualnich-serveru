#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.VirtualServers
{
    public class DeleteModel : PageModel
    {
        private readonly IDbService _dbService;

        public DeleteModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public VirtualServer VirtualServer { get; set; }

        public ActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualServer = _dbService.GetVirtualServer(id);

            if (VirtualServer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualServer = _dbService.GetVirtualServer(id);

            if (VirtualServer != null)
            {
                _dbService.RemoveVirtualServer(VirtualServer);
            }

            return RedirectToPage("./Index");
        }
    }
}
