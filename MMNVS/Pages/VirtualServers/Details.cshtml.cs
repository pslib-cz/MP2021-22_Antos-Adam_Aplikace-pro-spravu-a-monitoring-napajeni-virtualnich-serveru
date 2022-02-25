using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;
using Newtonsoft.Json.Linq;

namespace MMNVS.Pages.VirtualServers
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IVMService _vmService;

        public DetailsModel(ApplicationDbContext context, IVMService vmService)
        {
            _context = context;
            _vmService = vmService;
        }

        public VirtualServer VirtualServer { get; set; }
        public List<JToken> StateData { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

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
            try
            {
                StateData = _vmService.GetVirtualServerFromvCenter(VirtualServer.VMId);
                SuccessMessage = "Stavová data serveru úspěšně načtena.";
            }
            catch
            {
                ErrorMessage = "Chyba při načítání ze serveru vCenter.";
            }
            return Page();
        }
    }
}
