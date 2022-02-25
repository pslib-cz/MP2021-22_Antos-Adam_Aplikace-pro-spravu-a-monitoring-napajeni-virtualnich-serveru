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

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class DeleteModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;
        public int StorageServerId { get; set; }

        public DeleteModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Datastore Datastore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int storageServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            StorageServerId = storageServerId;
            Datastore = await _context.Datastores.FirstOrDefaultAsync(m => m.Id == id);

            if (Datastore == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int storageServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Datastore = await _context.Datastores.FindAsync(id);

            if (Datastore != null)
            {
                _context.Datastores.Remove(Datastore);
                await _context.SaveChangesAsync();
            }

            return Redirect("./Index?storageServerId=" + storageServerId);
        }
    }
}
