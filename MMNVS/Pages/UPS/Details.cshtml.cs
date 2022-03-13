#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class DetailsModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IUPSService _upsService;

        public DetailsModel(IDbService dbService, IUPSService upsService)
        {
            _dbService = dbService;
            _upsService = upsService;
        }

        public Model.UPS UPS { get; set; }
        public UPSLogItem UPSData {get; set;}

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UPS = _dbService.GetUPS(id);

            UPSData = _upsService.GetUPSLogItem(UPS);
            if (UPSData.Error == false) SuccessMessage = "Stavová data byla úspěšně načtena.";
            else ErrorMessage = "Data o stavu nelze načíst!";

            if (UPS == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}