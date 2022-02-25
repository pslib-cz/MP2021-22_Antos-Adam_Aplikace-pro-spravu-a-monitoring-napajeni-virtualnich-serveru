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

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IServerService _serverService;
        public int StorageServerId { get; set; }

        public EditModel(ApplicationDbContext context, IServerService serverService)
        {
            _context = context;
            _serverService = serverService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public Datastore Datastore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int storageServerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Datastore = await _context.Datastores.FirstOrDefaultAsync(m => m.Id == id);
            StorageServerId = storageServerId;

            if (Datastore == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int storageServerId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(Datastore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatastoreExists(Datastore.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (_serverService.DataStoreCheck(Datastore) == true) SuccessMessage = "Připojení k datastoru bylo úspěšné.";
            else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";

            return Redirect("./Index?storageServerId=" + storageServerId);
        }

        private bool DatastoreExists(int id)
        {
            return _context.Datastores.Any(e => e.Id == id);
        }
    }
}
