#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.VirtualServers
{
    public class CreateModel : PageModel
    {
        private readonly IDbService _dbService;

        public CreateModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public ActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public VirtualServer VirtualServer { get; set; }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _dbService.AddVirtualServer(VirtualServer);

            return RedirectToPage("./Index");
        }
    }
}
