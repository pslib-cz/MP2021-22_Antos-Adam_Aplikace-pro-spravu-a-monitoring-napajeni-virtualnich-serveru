#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts
{
    public class EditModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;

        public EditModel(IDbService dbService, IServerService serverService)
        {
            _dbService = dbService;
            _serverService = serverService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public HostServer HostServer { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostServer = _dbService.GetHostServer(id);

            if (HostServer == null)
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

            _dbService.EditItem(HostServer);

            if (HostServer.IsOSWindows == false)
            {
                if (_serverService.GetHostServerStatus(HostServer) == PowerStateEnum.PoweredOn) SuccessMessage = "Připojení k serveru bylo úspěšné.";
                else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
            }
            else
            {
                SuccessMessage = "Server s operačním systémem Windows byl upraven.";
            }


            return RedirectToPage("./Index");
        }
    }
}
