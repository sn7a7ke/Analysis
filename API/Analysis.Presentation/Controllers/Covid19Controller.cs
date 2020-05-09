using Analysis.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Covid19Controller : ControllerBase
    {
        private readonly ICovidAppService covidAppService;
        private readonly ILogger<Covid19Controller> logger;

        public Covid19Controller(ICovidAppService covidAppService, ILogger<Covid19Controller> logger)
        {
            this.covidAppService = covidAppService;
            this.logger = logger;
        }

        [HttpGet]
        public string GetJson()
        {
            return this.covidAppService.GetJson();
        }
    }
}
