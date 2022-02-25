using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;
using System.Security.Claims;

namespace MMNVS.Pages.ManageUsers
{
    public class DeleteModel : PageModel
    {
        private readonly IDbService _dbService;
        public MyUser MyUser { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public DeleteModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult OnGetAsync(string id)
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            if (id == userId)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            MyUser = _dbService.GetUser(id);

            if (MyUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            try
            {
                _dbService.RemoveUser(id);
                SuccessMessage = "Uživatel úspìšnì odstranìn";
            }
            catch
            {
                ErrorMessage = "Pøi odstraòovaní se vyskytla chyba";
            }
            return RedirectToPage("./Index");
        }
    }
}
