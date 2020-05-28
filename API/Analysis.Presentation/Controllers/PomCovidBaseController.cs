using System.Collections.Generic;
using Analysis.Application;
using Analysis.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(CacheProfileName = "default")]
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
        [Route("All")]
        public Dictionary<string, List<PomCovidInfo>> GetAll()
        {
            return this.pomCovidBaseAppService.GetAll();
        }

        [HttpGet]
        [Route("All/{country}")]
        public List<PomCovidInfo> GetByCountry(string country)
        {
            return this.pomCovidBaseAppService.GetByCountry(country);
        }

        [HttpGet]
        [Route("Summary")]
        public PomCovidInfo GetSummary()
        {
            return this.pomCovidBaseAppService.GetSummary();
        }

        [HttpGet]
        [Route("Summary/{country}")]
        public PomCovidInfo GetSummaryByCountry(string country)
        {
            return this.pomCovidBaseAppService.GetSummaryByCountry(country);
        }

        [HttpGet]
        [Route("AllCountries")]
        public List<string> GetAllCountries()
        {
            return this.pomCovidBaseAppService.GetAllCountries();
        }

        [HttpGet]
        [Route("LastDay/{country}")]
        public PomCovidInfo GetLastDayByCountry(string country)
        {
            return this.pomCovidBaseAppService.GetLastDayByCountry(country);
        }

    }
}
