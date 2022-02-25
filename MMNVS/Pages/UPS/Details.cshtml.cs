using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IUPSService _upsService;

        public DetailsModel(ApplicationDbContext context, IUPSService upsService)
        {
            _context = context;
            _upsService = upsService;
        }

        public Model.UPS UPS { get; set; }
        public UPSLogItem UPSData {get; set;}

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UPS = await _context.UPS.FirstOrDefaultAsync(m => m.Id == id);

            UPSData = _upsService.GetUPSLogItem(UPS);
            if (UPSData.Error == false) SuccessMessage = "Stavová data byla úspěšně načtena.";
            else ErrorMessage = "Data o stavu nelze načíst!";

            if (UPS == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}