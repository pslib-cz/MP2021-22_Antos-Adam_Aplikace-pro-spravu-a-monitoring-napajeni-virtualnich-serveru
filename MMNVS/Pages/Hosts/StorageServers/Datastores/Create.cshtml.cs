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

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IServerService _serverService;
        public VirtualStorageServer StorageServer { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public CreateModel(ApplicationDbContext context, IServerService serverService)
        {
            _context = context;
            _serverService = serverService;
        }

        public IActionResult OnGet(int storageServerId)
        {
            StorageServer = _context.VirtualStorageServers.FirstOrDefault(s => s.Id == storageServerId);
            return Page();
        }

        [BindProperty]
        public Datastore Datastore { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int storageServerId)
        {
            StorageServer = _context.VirtualStorageServers.FirstOrDefault(s => s.Id == storageServerId);
            Datastore.VirtualStorageServerId = StorageServer.Id;
            _context.Datastores.Add(Datastore);
            await _context.SaveChangesAsync();
            if (_serverService.DataStoreCheck(Datastore) == true) SuccessMessage = "Připojení k datastoru bylo úspěšné.";
            else ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
            return Redirect("./Index?storageServerId=" + storageServerId);
        }
    }
}
