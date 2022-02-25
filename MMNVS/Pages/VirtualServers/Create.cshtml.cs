using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Pages.VirtualServers
{
    public class CreateModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;

        public CreateModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public VirtualServer VirtualServer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            VirtualServer.Order = _context.VirtualServers.Count() + 1;
            _context.VirtualServers.Add(VirtualServer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
