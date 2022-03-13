#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages
{
    public class UPSLogModel : PageModel
    {
        private readonly IDbService _dbService;

        public UPSLogModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<UPSLogItem> UPSLogItem { get;set; }
        public int PagesCount { get; set; }
        public int PageNumber { get; set; }
        [BindProperty]
        public int ItemsPerPage { get; set; }

        public void OnGet(int pageNumber = 1, int itemsPerPage = 30)
        {
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;
            UPSLogItem = _dbService.GetUPSLog(itemsPerPage, pageNumber);
            PagesCount = _dbService.GetUPSLogPagesCount(itemsPerPage);
        }
        public ActionResult OnPost()
        {
            return Redirect("/UPSLog?pageNumber=1&itemsPerPage=" + ItemsPerPage);
        }
    }
}
