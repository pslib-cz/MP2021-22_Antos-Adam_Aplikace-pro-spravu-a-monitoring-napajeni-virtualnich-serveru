#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IServerService _serverService;
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public CreateModel(ApplicationDbContext context, IServerService serverService)
        {
            _context = context;
            _serverService = serverService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public HostServer HostServer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostSubmit()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.HostServers.Add(HostServer);
            await _context.SaveChangesAsync();
            if (_serverService.GetHostServerStatus(HostServer) == PowerStateEnum.PoweredOn) SuccessMessage = "Připojení k serveru bylo úspěšné.";
            else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostSubmitNewStorage()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.HostServers.Add(HostServer);
            await _context.SaveChangesAsync();

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
    }
}
