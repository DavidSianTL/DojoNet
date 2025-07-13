using EvaluacionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionApi.Controllers
{
    [ApiController]
    [Route("log")]
    public class LogController : Controller
    {
        private readonly LogService _log;

        public LogController(LogService log)
        {
            _log = log;
        }

        [HttpGet]
        public IActionResult ObtenerLog()
        {
            return Ok(_log.ObtenerLogs());
        }
    }
}
