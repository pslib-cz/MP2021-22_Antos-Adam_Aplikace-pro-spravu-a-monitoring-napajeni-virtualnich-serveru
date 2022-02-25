using Microsoft.AspNetCore.Mvc;
using MMNVS.Services;

namespace MMNVS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpsController : ControllerBase
    {
        private readonly IScenarioService _scenarioService;

        public UpsController(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        [HttpGet]
        public string Get()
        {
            _scenarioService.CheckUps();
            return "Success";
        }

    }
}
