#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;

        public IndexModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<HostServer> HostServer { get;set; }

        public void OnGet()
        {
            HostServer = _dbService.GetHostServers();
        }
    }
}
