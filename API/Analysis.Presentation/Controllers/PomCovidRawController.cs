using Analysis.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PomCovidRawController : ControllerBase
    {
        private readonly IRawAppService covidAppService;
        private readonly ILogger<PomCovidRawController> logger;

        public PomCovidRawController(IRawAppService covidAppService, ILogger<PomCovidRawController> logger)
        {
            this.covidAppService = covidAppService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("Json")]
        public string GetJson()
        {
            return this.covidAppService.Json;
        }

        [HttpGet]
        [Route("RefreshData")]
        public bool GetRefreshDataJson()
        {
            return this.covidAppService.RefreshDataAsync().Result;            
        }
    }
}
