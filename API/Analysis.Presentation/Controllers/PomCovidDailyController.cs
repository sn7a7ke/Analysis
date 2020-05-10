using Analysis.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PomCovidDailyController : PomCovidBaseController
    {
        public PomCovidDailyController(PomDailyAppService pomCovidDailyAppService, ILogger<PomCovidDailyController> logger) :
            base(pomCovidDailyAppService, logger)
        {
        }
    }
}
