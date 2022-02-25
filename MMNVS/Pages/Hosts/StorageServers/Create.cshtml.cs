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

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IServerService _serverService;
        public HostServer HostServer { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public CreateModel(ApplicationDbContext context, IServerService serverService)
        {
            _context = context;
            _serverService = serverService;
        }

        public IActionResult OnGet(int hostServerId)
        {
        //ViewData["HostId"] = new SelectList(_context.HostServers, "Id", "Id");
            HostServer = _context.HostServers.FirstOrDefault(s => s.Id == hostServerId);
            return Page();
        }

        [BindProperty]
        public VirtualStorageServer VirtualStorageServer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost(int hostServerId)
        {
            HostServer = _context.HostServers.FirstOrDefault(s => s.Id == hostServerId);
            VirtualStorageServer.HostId = HostServer.Id;
            _context.VirtualStorageServers.Add(VirtualStorageServer);
            _context.SaveChanges();

            if (_serverService.GetStorageServerStatus(VirtualStorageServer) == PowerStateEnum.PoweredOn)
            {
                SuccessMessage = "Připojení k serveru bylo úspěšné.";
                return Redirect("./Datastores/Create?storageServerId=" + VirtualStorageServer.Id.ToString());
            }

            else
            {
                ErrorMessage = "Při pokusu o připojení se vyskytla chyba!";
                return Redirect("./Index?hostServerId=" + HostServer.Id.ToString());
            }
        }
    }
}
