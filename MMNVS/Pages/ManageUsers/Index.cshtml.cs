using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;
using System.Security.Claims;

namespace MMNVS.Pages.ManageUsers
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;
        public List<MyUser> Users { get; set; }
        public string UserId { get; set; }

        public IndexModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public void OnGet()
        {
            Users = _dbService.GetUsers();
            UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
        }
    }
}
