using System.Diagnostics;
using ExamDaniel.Models;
using Microsoft.AspNetCore.Mvc;
using ExamDaniel.bitacora; 

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
                // Registro en bitácora por intento de acceso sin sesión
                BitacoraManager.RegistrarEvento("Acceso", "Intento de acceso sin sesión activa");
                return RedirectToAction("Login", "Login");
            }

            // Registro en bitácora por acceso exitoso
            BitacoraManager.RegistrarEvento("Acceso", $"Usuario {usrNombre} accedió al Home");

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
