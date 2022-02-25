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

namespace MMNVS.Pages.Hosts.StorageServers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public HostServer HostServer { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<VirtualStorageServer> VirtualStorageServer { get;set; }

        public async Task OnGetAsync(int hostServerId)
        {
            VirtualStorageServer = await _context.VirtualStorageServers
                .Include(v => v.Host).Where(s => s.HostId == hostServerId).ToListAsync();
            HostServer = await _context.HostServers.FirstOrDefaultAsync(h => h.Id == hostServerId);
        }
    }
}
