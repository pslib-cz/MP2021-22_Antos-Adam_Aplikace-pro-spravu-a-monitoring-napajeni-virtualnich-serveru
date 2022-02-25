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

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IServerService _serverService;
        public int HostServerId { get; set; }

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
        public VirtualStorageServer VirtualStorageServer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int hostServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostServerId = hostServerId;
            VirtualStorageServer = await _context.VirtualStorageServers
                .Include(v => v.Host).FirstOrDefaultAsync(m => m.Id == id);

            if (VirtualStorageServer == null)
            {
                return NotFound();
            }
           ViewData["HostId"] = new SelectList(_context.HostServers, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int hostServerId)
        {
            _context.Attach(VirtualStorageServer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VirtualStorageServerExists(VirtualStorageServer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (_serverService.GetStorageServerStatus(VirtualStorageServer) == PowerStateEnum.PoweredOn)
            {
                SuccessMessage = "Připojení k serveru bylo úspěšné.";
            }

            else
            {
                ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
            }

            return Redirect("./Index?hostServerId=" + hostServerId);
        }

        private bool VirtualStorageServerExists(int id)
        {
            return _context.VirtualStorageServers.Any(e => e.Id == id);
        }
    }
}
