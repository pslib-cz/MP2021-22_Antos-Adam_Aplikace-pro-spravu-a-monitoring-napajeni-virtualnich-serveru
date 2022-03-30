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
            UPSLogItem = _dbService.GetUPSLog(DateTimeFromTo.DateTimeTo, DateTimeFromTo.DateTimeFrom, itemsPerPage, pageNumber);
            PagesCount = _dbService.GetUPSLogPagesCount(DateTimeFromTo.DateTimeTo, DateTimeFromTo.DateTimeFrom, itemsPerPage);
        }
        public ActionResult OnPost(string dateTimeTo, string dateTimeFrom)
        {
            return Redirect("/UPSLog?dateTimeTo=" + dateTimeTo + "&dateTimeFrom="+ dateTimeFrom +"&itemsPerPage=" + ItemsPerPage);
        }
        public ActionResult OnPostDate(int itemsPerPage = 50)
        {
            return Redirect("/UPSLog?dateTimeTo=" + DateTimeFromTo.DateTimeTo.ToString("yyyyMMddHHmmss") + "&dateTimeFrom=" + DateTimeFromTo.DateTimeFrom.ToString("yyyyMMddHHmmss") + "&itemsPerPage=" + itemsPerPage);
        }
    }
}
