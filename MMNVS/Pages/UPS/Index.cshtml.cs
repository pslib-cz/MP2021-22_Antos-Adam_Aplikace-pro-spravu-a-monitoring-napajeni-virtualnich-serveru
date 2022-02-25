using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class IndexModel : PageModel
    {
        private readonly IDbService _dbService;

        public IndexModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<Model.UPS> UPS { get;set; }

        public async Task OnGetAsync()
        {
            UPS = _dbService.GetUPSs();
        }
    }
}
