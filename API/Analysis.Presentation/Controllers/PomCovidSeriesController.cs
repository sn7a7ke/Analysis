using Analysis.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PomCovidSeriesController : PomCovidBaseController
    {
        public PomCovidSeriesController(PomSeriesAppService pomCovidSeriesAppService, ILogger<PomCovidSeriesController> logger) :
            base(pomCovidSeriesAppService, logger)
        {
        }
    }
}
