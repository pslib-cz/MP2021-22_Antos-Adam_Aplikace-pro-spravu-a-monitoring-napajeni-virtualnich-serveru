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

        [Display(Name = "U�ivatelsk� jm�no")]
        [BindProperty]
        public string UserName { get; set; }
        [Display(Name = "Heslo")]
        [BindProperty]
        public string Password { get; set; }
        [Display(Name = "Pozn�mka")]
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
                SuccessMessage = "U�ivatel " + UserName + " byl �sp�n� p�id�n.";
            }
            catch
            {
                ErrorMessage = "P�i p�id�v�n� u�ivatele se vyskytla chyba!";
            }

            return RedirectToPage("./Index");
        }
    }
}
