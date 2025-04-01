using System.Diagnostics;
using MI_PRIMERA_APP_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace MI_PRIMERA_APP_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Privacy()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Mesaje = "usuario registrado con exito";
                    return View("index", usuario_ejemplo);
                     }
                else {
                    ViewBag.Mesaje = "usuario registrado con exito";
                    return View("index", usuario_ejemplo);
                }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
