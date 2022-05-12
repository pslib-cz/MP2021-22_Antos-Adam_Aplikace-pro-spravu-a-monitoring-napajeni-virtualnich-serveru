#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts
{
    public class CreateModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;

        public CreateModel(IDbService dbService, IServerService serverService)
        {
            _dbService = dbService;
            _serverService = serverService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public ActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public HostServer HostServer { get; set; }

        public ActionResult OnPostSubmit()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbService.AddItem(HostServer);

            if (HostServer.IsOSWindows == false)
            {
                if (_serverService.GetHostServerStatus(HostServer) == PowerStateEnum.PoweredOn) SuccessMessage = "Připojení k serveru bylo úspěšné.";
                else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
            }

            else
            {
                SuccessMessage = "Server s operačním systémem Windows byl přidán.";
            }

            return RedirectToPage("./Index");
        }

        public ActionResult OnPostSubmitNewStorage()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbService.AddItem(HostServer);

            if (HostServer.IsOSWindows == false)
            {
                if (_serverService.GetHostServerStatus(HostServer) == PowerStateEnum.PoweredOn)
                {
                    SuccessMessage = "Připojení k serveru bylo úspěšné. Pokračujte k přidání storage serveru.";
                    return Redirect("./StorageServers/Create?hostServerId=" + HostServer.Id.ToString());
                }

                else
                {
                    ErrorMessage = "Při pokusu o připojení se vyskytla chyba! Před pokračováním dále zkontrolujte nastavení hostitelského serveru.";
                    return RedirectToPage("./Index");
                }
            }

            else
            {
                SuccessMessage = "Server s operačním systémem Windows byl přidán.";
                return RedirectToPage("./Index");
            }
        }
    }
}
