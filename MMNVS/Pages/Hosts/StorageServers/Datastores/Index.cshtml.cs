#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;

        public IndexModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public IList<Datastore> Datastore { get;set; }
        public VirtualStorageServer StorageServer { get;set; }

        public void OnGet(int storageServerId)
        {
            StorageServer = _dbService.GetStorageServer(storageServerId);
            Datastore = _dbService.GetDatastores(storageServerId);
        }
    }
}
