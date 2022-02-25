using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MMNVS.Data;
using MMNVS.Model;

namespace MMNVS.Pages.UPS
{
    public class CreateModel : PageModel
    {
        private readonly MMNVS.Data.ApplicationDbContext _context;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public CreateModel(MMNVS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Model.UPS UPS { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var dataUps = Messenger.Get(VersionCode.V1,
                   new IPEndPoint(IPAddress.Parse(UPS.IPAddress), 161),
                   new OctetString("public"),
                   new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.4.1.232.165.1.2.2.0")), new Variable(new ObjectIdentifier("1.3.6.1.4.1.232.165.1.2.1.0")) },
                   6000);
                UPS.Model = dataUps[0].Data.ToString();
                UPS.Producer = dataUps[1].Data.ToString();
            }
            catch (Exception exception)
            {
                ErrorMessage = "Informace o modelu a výrobci se pomocí protokolu SNMP nepodařilo zjistit!";
            }
            _context.UPS.Add(UPS);
            SuccessMessage = "UPS " + UPS.Producer + " byla přidána.";
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
