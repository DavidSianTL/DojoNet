using System.Diagnostics;
using CRUD_Evaluacion_Mensual_Abril.Models;
using CRUD_Evaluacion_Mensual_Abril.Service;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Evaluacion_Mensual_Abril.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConfiguracionService _configService;

        public HomeController(ConfiguracionService configService)
        {
            _configService = configService;
        }

        public IActionResult Index()
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.usrNombre = usrNombre;
            ViewBag.NombreCompleto = NombreCompleto;

            // Leer color desde el JSON
            var config = _configService.ObtenerConfiguracion();
            ViewBag.ColorPrincipal = config.ColorPrincipal;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
