using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.VirtualServers
{
    public class IndexModel : PageModel
    {
        private readonly IVMService _vmService;
        private readonly IDbService _dbService;

        public IndexModel(IVMService vmService, IDbService dbService)
        {
            _vmService = vmService;
            _dbService = dbService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public IList<VirtualServer> VirtualServer { get;set; }

        public async Task OnGetAsync()
        {
            VirtualServer = _dbService.GetVirtualServers();
        }

        public IActionResult OnGetGetFromvCenter()
        {
            try
            {
                _vmService.GetVirtualServersFromvCenter();
                SuccessMessage = "Seznam virtuálních serverů úspěšně načten ze serveru vCenter.";
            }
            catch
            {
                ErrorMessage = "Chyba při načítání ze serveru vCenter.";
            }
            return RedirectToPage("Index");
        }

        public IActionResult OnGetChangeOrderUp(string vmid)
        {
            VirtualServer vm = _dbService.GetVirtualServer(vmid);
            VirtualServer vmUp = _dbService.GetVirtualServerByOrder(vm.Order - 1);
            if (vmUp != null)
            {
                vm.Order -= 1;
                vmUp.Order += 1;
                _dbService.EditItem(vm);
                _dbService.EditItem(vmUp);
                SuccessMessage = "Pořadí úspěšně změněno.";
            }
            else ErrorMessage = "Pořadí nelze zvýšit.";
            return RedirectToPage("Index");
        }
        public IActionResult OnGetChangeOrderDown(string vmid)
        {
            VirtualServer vm = _dbService.GetVirtualServer(vmid);
            VirtualServer vmDown = _dbService.GetVirtualServerByOrder(vm.Order + 1);
            if (vmDown != null)
            {
                vm.Order += 1;
                vmDown.Order -= 1;
                _dbService.EditItem(vm);
                _dbService.EditItem(vmDown);
                SuccessMessage = "Pořadí úspěšně změněno.";
            }
            else ErrorMessage = "Pořadí nelze snížit.";
            return RedirectToPage("Index");
        }
    }
}
