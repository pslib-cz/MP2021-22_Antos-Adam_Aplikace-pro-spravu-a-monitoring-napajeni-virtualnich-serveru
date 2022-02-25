using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Pages.VirtualServers
{
    public class DeleteModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;

        public DeleteModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VirtualServer VirtualServer { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualServer = await _context.VirtualServers.FirstOrDefaultAsync(m => m.VMId == id);

            if (VirtualServer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualServer = await _context.VirtualServers.FindAsync(id);

            if (VirtualServer != null)
            {
                _context.VirtualServers.Remove(VirtualServer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
