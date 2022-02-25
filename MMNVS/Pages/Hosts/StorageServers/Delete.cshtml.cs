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

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class DeleteModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;
        public int HostServerId { get; set; }
        public DeleteModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VirtualStorageServer VirtualStorageServer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int hostServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualStorageServer = await _context.VirtualStorageServers
                .Include(v => v.Host).FirstOrDefaultAsync(m => m.Id == id);

            if (VirtualStorageServer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, int hostServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualStorageServer = await _context.VirtualStorageServers.Include(d => d.Datastores).FirstOrDefaultAsync(v => v.Id == id);

            if (VirtualStorageServer != null)
            {
                foreach (Datastore datastore in VirtualStorageServer.Datastores)
                {
                    _context.Datastores.Remove(datastore);
                }
                _context.VirtualStorageServers.Remove(VirtualStorageServer);
                await _context.SaveChangesAsync();
            }

            return Redirect("./Index?hostServerId=" + hostServerId);
        }
    }
}
