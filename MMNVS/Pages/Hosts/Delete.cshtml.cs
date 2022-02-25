#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Pages.Hosts
{
    public class DeleteModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;

        public DeleteModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HostServer = await _context.HostServers.Include(h => h.VirtualStorageServers).ThenInclude(g => g.Datastores).FirstOrDefaultAsync(s => s.Id == id);

            if (HostServer != null)
            {
                foreach (VirtualStorageServer storageServer in HostServer.VirtualStorageServers)
                {
                    foreach (Datastore datastore in storageServer.Datastores)
                    {
                        _context.Datastores.Remove(datastore);
                    }
                    _context.VirtualStorageServers.Remove(storageServer);
                }
                _context.HostServers.Remove(HostServer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
