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
        public string DateTimeTo { get; set; }
        public string DateTimeFrom { get; set; }
        [BindProperty]
        public DateTimeFromTo DateTimeFromTo { get; set; }

        public void OnGet(string dateTimeTo, string dateTimeFrom, int pageNumber = 1, int itemsPerPage = 50)
        {
            DateTimeTo = dateTimeTo;
            DateTimeFrom = dateTimeFrom;
            DateTimeFromTo = _dbService.CheckDateNotNull(DateTimeTo, DateTimeFrom);
            PageNumber = pageNumber;
            ItemsPerPage = itemsPerPage;
            LogItem = _dbService.GetLog(DateTimeFromTo.DateTimeTo, DateTimeFromTo.DateTimeFrom, itemsPerPage, pageNumber);
            PagesCount = _dbService.GetLogPagesCount(DateTimeFromTo.DateTimeTo, DateTimeFromTo.DateTimeFrom, itemsPerPage);
        }

        public ActionResult OnPost(DateTime? dateTimeTo, DateTime? dateTimeFrom)
        {
            return Redirect("/Log?dateTimeTo=" + dateTimeTo + "&dateTimeFrom=" + dateTimeFrom + "&itemsPerPage=" + ItemsPerPage);
        }
        public ActionResult OnPostDate(int itemsPerPage = 50)
        {
            return Redirect("/Log?dateTimeTo=" + DateTimeFromTo.DateTimeTo.ToString("yyyyMMddHHmmss") + "&dateTimeFrom=" + DateTimeFromTo.DateTimeFrom.ToString("yyyyMMddHHmmss") + "&itemsPerPage=" + itemsPerPage);
        }
    }
}
