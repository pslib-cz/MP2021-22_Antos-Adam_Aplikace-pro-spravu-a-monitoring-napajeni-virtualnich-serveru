#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class CreateModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IUPSService _upsService;

        public CreateModel(IDbService dbService, IUPSService upsService)
        {
            _dbService = dbService;
            _upsService = upsService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Model.UPS UPS { get; set; }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                UPS = _upsService.GetUPSProducerModel(UPS);
            }
            catch
            {
                ErrorMessage = "Informace o modelu a výrobci se pomocí protokolu SNMP nepodařilo zjistit!";
            }
            _dbService.AddItem(UPS);
            SuccessMessage = "UPS " + UPS.Producer + " byla přidána.";

            return RedirectToPage("./Index");
        }
    }
}
