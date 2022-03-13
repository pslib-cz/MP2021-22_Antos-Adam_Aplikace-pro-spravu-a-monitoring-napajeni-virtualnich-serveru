#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class EditModel : PageModel
    {
        private readonly IDbService _dbService;

        public EditModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Model.UPS UPS { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UPS = _dbService.GetUPS(id);

            if (UPS == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbService.EditItem(UPS);

            return RedirectToPage("./Index");
        }
    }
}
