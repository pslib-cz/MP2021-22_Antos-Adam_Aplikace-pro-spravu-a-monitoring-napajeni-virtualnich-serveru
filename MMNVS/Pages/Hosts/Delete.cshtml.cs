#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts
{
    public class DeleteModel : PageModel
    {
        private readonly IDbService _dbService;

        public DeleteModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public HostServer HostServer { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostServer = _dbService.GetHostServer(id);

            if (HostServer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _dbService.RemoveHostServer(id);

            return RedirectToPage("./Index");
        }
    }
}
