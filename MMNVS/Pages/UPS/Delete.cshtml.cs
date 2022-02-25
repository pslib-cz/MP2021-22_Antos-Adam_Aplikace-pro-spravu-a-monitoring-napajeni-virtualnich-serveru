using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Pages.UPS
{
    public class DeleteModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;

        public DeleteModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Model.UPS UPS { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UPS = await _context.UPS.FirstOrDefaultAsync(m => m.Id == id);

            if (UPS == null)
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

            UPS = await _context.UPS.FindAsync(id);

            if (UPS != null)
            {
                _context.UPS.Remove(UPS);
                foreach (UPSLogItem item in _context.UPSLog.Where(u => u.UPSId == UPS.Id))
                {
                    _context.UPSLog.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
