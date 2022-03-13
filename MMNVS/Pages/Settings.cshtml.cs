#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IMailService _mailService;
        private readonly IVMService _vmService;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public SettingsModel(IDbService dbService, IMailService mailService, IVMService vmService)
        {
            _dbService = dbService;
            _mailService = mailService;
            _vmService = vmService;
        }

        [BindProperty]
        public AppSettings Settings { get; set; }

        public ActionResult OnGet()
        {

            Settings = _dbService.GetSettings();

            ViewData["PrimaryUPSId"] = new SelectList(_dbService.GetUPSs(), "Id", "Name");            
            return Page();
        }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _dbService.UpdateSettings(Settings);
            if (_vmService.GetvCenterState() == PowerStateEnum.PoweredOn) SuccessMessage = "Připojení k vCenter serveru bylo úspěšné. Změny byly uloženy.";
            else ErrorMessage = "Při připojování k serveru se vyskytla neočekávaná chyba, zkontrolujte správnost údajů!";
            //return RedirectToPage("Settings");
            return RedirectToPage();
        }

        public ActionResult OnPostTestMail()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _dbService.UpdateSettings(Settings);
                _mailService.SendMail("Testovací zpráva MMNVS", "Pokud Vám byl doručen tento email, Vaše nastavení SMTP je správné.");
                SuccessMessage = "Testovací email byl odeslán";
            }
            catch
            {
                ErrorMessage = "Vyskytla neočekávaná chyba, překontrolujte nastavení SMTP!";
            }
            return RedirectToPage();
        }
    }
}
