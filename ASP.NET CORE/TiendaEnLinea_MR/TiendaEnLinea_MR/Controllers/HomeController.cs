using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TiendaEnLinea_MR.Models;

namespace TiendaEnLinea_MR.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            //verificamos si el usuario esta logeado
            var Correo = HttpContext.Session.GetString("Correo");
            var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (Correo == null)
            {
                return RedirectToAction("Login", "Login");


            }
            ViewBag.Correo = Correo;
            ViewBag.NombreCompleto = NombreCompleto;

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
