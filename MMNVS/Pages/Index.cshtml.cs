using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MMNVS.Data;
using MMNVS.Model;
using MMNVS.Services;

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

        public async void OnGet()
        {
            Settings = _dbService.GetSettingsWithoutInclude();
            //_vmService.ShutdownVirtualServer("vm-110");

            //apiKey = _VMService.GetvCenterApiKey().Result;

            //_dbService.GetSettings().vCenterIP = "69";
            //test = _vmService.StartVirtualServer("vm-5838").Result.ToString();
            //test = _vmService.GetPowerVirtualServer("vm-110").ToString();

            //test = "vCenter na hostu: " + _serverService.FindvCenter().Name;
            //_mailService.SendMail("Test1", "Errorrrrrr");
        }
    }
}