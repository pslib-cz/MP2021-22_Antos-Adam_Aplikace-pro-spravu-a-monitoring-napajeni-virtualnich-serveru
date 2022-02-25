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

namespace MMNVS.Pages.Hosts
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IServerService _serverService;
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public EditModel(ApplicationDbContext context, IServerService serverService)
        {
            _context = context;
            _serverService = serverService;
        }

        [BindProperty]
        public HostServer HostServer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostServer = await _context.HostServers.FirstOrDefaultAsync(m => m.Id == id);

            if (HostServer == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(HostServer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostServerExists(HostServer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            if (_serverService.GetHostServerStatus(HostServer) == PowerStateEnum.PoweredOn) SuccessMessage = "Připojení k serveru bylo úspěšné.";
            else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";

            return RedirectToPage("./Index");
        }

        private bool HostServerExists(int id)
        {
            return _context.HostServers.Any(e => e.Id == id);
        }
    }
}
