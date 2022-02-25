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

        public string apiUrl { get; set; }
        public string test { get; set; }

        //private readonly AppSettings _appSettings;



        public async void OnGet()
        {
            //apiKey = _VMService.GetvCenterApiKey().Result;
            apiUrl = _dbService.GetSettings().vCenterApiUrl;
            //_dbService.GetSettings().vCenterIP = "69";
            //test = _vmService.StartVirtualServer("vm-5838").Result.ToString();
            test = _vmService.GetPowerVirtualServer("vm-5838").ToString();
            //test = "vCenter na hostu: " + _serverService.FindvCenter().Name;
            //_mailService.SendMail("Test1", "Errorrrrrr");
        }
    }
}