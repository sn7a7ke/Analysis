using System.Collections.Generic;
using Analysis.Application;
using Analysis.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class PomCovidBaseController : ControllerBase
    {
        private readonly PomBaseAppService pomCovidBaseAppService;
        private readonly ILogger<PomCovidBaseController> logger;

        public PomCovidBaseController(PomBaseAppService pomCovidBaseAppService, ILogger<PomCovidBaseController> logger)
        {
            this.pomCovidBaseAppService = pomCovidBaseAppService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("GetAll")]
        public Dictionary<string, List<PomCovidInfo>> GetAll()
        {
            return this.pomCovidBaseAppService.GetAll();
        }

        [HttpGet]
        [Route("GetByCountry")]
        public List<PomCovidInfo> GetByCountry(string country)
        {
            return this.pomCovidBaseAppService.GetByCountry(country);
        }

        [HttpGet]
        [Route("GetSummaryByCountry")]
        public PomCovidInfo GetSummaryByCountry(string country)
        {
            return this.pomCovidBaseAppService.GetSummaryByCountry(country);
        }
    }
}
