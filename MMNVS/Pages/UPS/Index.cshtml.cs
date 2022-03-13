#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;

        public IndexModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<Model.UPS> UPS { get;set; }

        public void OnGet()
        {
            UPS = _dbService.GetUPSs();
        }
    }
}
