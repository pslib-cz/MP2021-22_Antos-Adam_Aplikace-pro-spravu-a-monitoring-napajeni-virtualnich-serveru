#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.VirtualServers
{
    public class EditModel : PageModel
    {
        private readonly IDbService _dbService;

        public EditModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public VirtualServer VirtualServer { get; set; }

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
            return Page();
        }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbService.EditItem(VirtualServer);

            return RedirectToPage("./Index");
        }

        public ActionResult OnPostSetvCenter()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _dbService.SetvCenter(VirtualServer);
                SuccessMessage = "Server " + VirtualServer.Name + " byl nastaven jako server vCenter";
            }
            catch
            {
                ErrorMessage = "Chyba!";
            }

            return RedirectToPage("./Index");
        }
    }
}
