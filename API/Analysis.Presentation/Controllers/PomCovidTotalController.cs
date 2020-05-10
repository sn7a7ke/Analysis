using Analysis.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PomCovidTotalController : PomCovidBaseController
    {
        public PomCovidTotalController(PomTotalAppService pomCovidTotalAppService, ILogger<PomCovidTotalController> logger) : 
            base(pomCovidTotalAppService, logger)
        {
        }
    }
}
