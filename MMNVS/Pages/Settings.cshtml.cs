#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbService _dbService;
        private readonly IMailService _mailService;
        private readonly IVMService _vmService;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public SettingsModel(ApplicationDbContext context, IDbService dbService, IMailService mailService, IVMService vmService)
        {
            _context = context;
            _dbService = dbService;
            _mailService = mailService;
            _vmService = vmService;
        }

        [BindProperty]
        public AppSettings AppSettings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            AppSettings = _dbService.GetSettings();

           ViewData["PrimaryUPSId"] = new SelectList(_context.UPS, "Id", "Name");            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _dbService.UpdateSettings(AppSettings);
            if (_vmService.GetvCenterState() == PowerStateEnum.PoweredOn) SuccessMessage = "Připojení k vCenter serveru bylo úspěšné. Změny byly uloženy.";
            else ErrorMessage = "Při připojování k serveru se vyskytla neočekávaná chyba, zkontrolujte správnost údajů!";
            return RedirectToPage("Settings");
        }

        public async Task<IActionResult> OnPostTestMail()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _dbService.UpdateSettings(AppSettings);
                _mailService.SendMail("Testovací zpráva MMNVS", "Pokud Vám byl doručen tento email, Vaše nastavení SMTP je správné.");
                SuccessMessage = "Testovací email byl odeslán";
            }
            catch
            {
                ErrorMessage = "Vyskytla neočekávaná chyba, překontrolujte nastavení SMTP!";
            }
            return RedirectToPage("Settings");
        }
    }
}
