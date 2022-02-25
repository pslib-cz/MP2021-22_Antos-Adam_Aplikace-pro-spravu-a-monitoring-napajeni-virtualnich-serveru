#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Pages.Hosts.StorageServers.Datastores
{
    public class IndexModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;

        public IndexModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Datastore> Datastore { get;set; }
        public VirtualStorageServer StorageServer { get;set; }

        public async Task OnGetAsync(int storageServerId)
        {
            StorageServer = await _context.VirtualStorageServers.FirstOrDefaultAsync(s => s.Id == storageServerId);
            Datastore = await _context.Datastores.Where(d => d.VirtualStorageServerId == storageServerId).ToListAsync();
        }
    }
}
