using System.Diagnostics;
using ExamDaniel.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamDaniel.Controllers
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
            // Verificamos si el usuario está logueado buscando su nombre de usuario en sesión
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (string.IsNullOrEmpty(usrNombre))
            {
                // Si no está logueado, lo redirigimos a la pantalla de login
                return RedirectToAction("Login", "Login");
            }

            
            ViewBag.UsrNombre = usrNombre;
            ViewBag.NombreCompleto = nombreCompleto;

            return View();
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
