#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages
{
    public class LogModel : PageModel
    {
        private readonly IDbService _dbService;

        public LogModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public IList<LogItem> LogItem { get; set; }
        public int PagesCount { get; set; }
        public int PageNumber { get; set; }
        [BindProperty]
        public int ItemsPerPage { get; set; }

        public void OnGet(int pageNumber = 1, int itemsPerPage = 30)
        {
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;
            LogItem = _dbService.GetLog(itemsPerPage, pageNumber);
            PagesCount = _dbService.GetLogPagesCount(itemsPerPage);
        }

        public ActionResult OnPost()
        {
            return Redirect("/Log?pageNumber=1&itemsPerPage=" + ItemsPerPage);
        }
    }
}
