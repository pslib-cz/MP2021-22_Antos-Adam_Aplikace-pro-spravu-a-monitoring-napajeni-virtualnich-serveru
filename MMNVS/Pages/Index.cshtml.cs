using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MMNVS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IVMService _vmService;
        private readonly IDbService _dbService;
        private readonly IServerService _serverService;
        private readonly IMailService _mailService;

        public IndexModel(IVMService vmService, IDbService dbService, IServerService serverService, IMailService mailService)
        {
            _vmService = vmService;
            _dbService = dbService;
            _serverService = serverService;
            _mailService = mailService;
        }

        public AppSettings Settings { get; set; }
        public PowerStateEnum Test { get; set; }

        public async void OnGet()
        {
            Settings = _dbService.GetSettingsWithoutInclude();

            Test = _serverService.GetHostServeriLOStatus(_dbService.GetHostServer(1)).Result;
            //_serverService.StartHost(_dbService.GetHostServer(1));
        }
    }
}