using System;
using System.Collections.Generic;
using System.Linq;
#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MMNVS.Services;

namespace MMNVS.Pages.UPS
{
    public class DeleteModel : PageModel
    {
        private readonly IDbService _dbService;

        public DeleteModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Model.UPS UPS { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UPS = _dbService.GetUPS(id);

            if (UPS == null)
            {
                return NotFound();
            }
            return Page();
        }

        public ActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _dbService.RemoveUPS(id);

            return RedirectToPage("./Index");
        }
    }
}
