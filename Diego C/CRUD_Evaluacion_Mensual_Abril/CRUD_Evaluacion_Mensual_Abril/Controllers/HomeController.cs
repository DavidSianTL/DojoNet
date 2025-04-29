using System.Diagnostics;
using CRUD_Evaluacion_Mensual_Abril.Models;
using CRUD_Evaluacion_Mensual_Abril.Service;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Evaluacion_Mensual_Abril.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServicioSesion _servicioSesion;

        public HomeController(ILogger<HomeController> logger, IServicioSesion servicioSesion)
        {
            _logger = logger;
            _servicioSesion = servicioSesion;
        }

        public IActionResult Index()
        {
            if (!_servicioSesion.EsSesionValida())
            {
                TempData["MensajeSesion"] = "Debes iniciar sesion para continuar";
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            if (!_servicioSesion.EsSesionValida())
            {
                TempData["MensajeSesion"] = "Debes iniciar sesion para continuar";
                return RedirectToAction("Index", "Login");
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
