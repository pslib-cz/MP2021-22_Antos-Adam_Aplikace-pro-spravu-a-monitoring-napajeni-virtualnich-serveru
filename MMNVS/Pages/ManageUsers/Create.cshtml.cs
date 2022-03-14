#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;
using System.ComponentModel.DataAnnotations;

namespace MMNVS.Pages.ManageUsers
{
    public class CreateModel : PageModel
    {
        private readonly IDbService _dbService;

        public CreateModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [Display(Name = "Uživatelské jméno")]
        [BindProperty]
        public string UserName { get; set; }
        [Display(Name = "Heslo")]
        [BindProperty]
        public string Password { get; set; }
        [Display(Name = "Poznámka")]
        [BindProperty]
        public string Notes { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public ActionResult OnPost()
        {
            try
            {
                _dbService.AddUser(UserName, Password, Notes);
                SuccessMessage = "Uživatel " + UserName + " byl úspìšnì pøidán.";
            }
            catch
            {
                ErrorMessage = "Pøi pøidávání uživatele se vyskytla chyba!";
            }

            return RedirectToPage("./Index");
        }
    }
}
