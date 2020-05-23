using System.Collections.Generic;
using Analysis.Application;
using Analysis.Communication.Pom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Analysis.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PomCovidTableController : ControllerBase
    {
        private readonly PomTableAppService pomTableAppService;
        private readonly ILogger<PomCovidTableController> logger;

        public PomCovidTableController(PomTableAppService pomTableAppService, ILogger<PomCovidTableController> logger)
        {
            this.pomTableAppService = pomTableAppService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("All")]
        public List<PomTableDTO> GetAll()
        {
            return this.pomTableAppService.GetAll();
        }
    }
}
