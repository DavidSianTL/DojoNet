using System.Diagnostics;
using ExamenUno.Models;
using Microsoft.AspNetCore.Mvc;
using ExamenUno.Services;


namespace ExamenUno.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;

        public HomeController(ILogger<HomeController> logger, ISessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
