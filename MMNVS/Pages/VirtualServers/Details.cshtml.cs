#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;
using Newtonsoft.Json.Linq;

namespace MMNVS.Pages.VirtualServers
{
    public class DetailsModel : PageModel
    {
        private readonly IDbService _dbService;
        private readonly IVMService _vmService;

        public DetailsModel(IDbService dbService, IVMService vmService)
        {
            _dbService = dbService;
            _vmService = vmService;
        }

        public VirtualServer VirtualServer { get; set; }
        public List<JToken> StateData { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public ActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VirtualServer = _dbService.GetVirtualServer(id);

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
