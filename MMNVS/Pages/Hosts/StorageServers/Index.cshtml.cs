#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;

        public IndexModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public HostServer HostServer { get; set; }
        public IList<VirtualStorageServer> VirtualStorageServers { get;set; }

        public void OnGet(int hostServerId)
        {
            VirtualStorageServers = _dbService.GetStorageServers(hostServerId);
            HostServer = _dbService.GetHostServer(hostServerId);
        }
    }
}
