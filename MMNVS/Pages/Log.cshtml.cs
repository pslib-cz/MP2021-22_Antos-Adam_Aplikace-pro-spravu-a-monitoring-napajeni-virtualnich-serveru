﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

namespace MMNVS.Pages
{
    public class LogModel : PageModel
    {
        private readonly IDbService _dbService;

        public LogModel(IDbService dbService)
        {
            _dbService = dbService;
        }

        public IList<LogItem> LogItem { get;set; }

        public void OnGet()
        {
            LogItem = _dbService.GetLog();
        }
    }
}
