using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WbPruebaCSS.Models;

namespace WbPruebaCSS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // Simularemos una base de datos con una lista de empleados.
        private static List<Empleado_Modeller> empleados = new List<Empleado_Modeller>();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Pasar la lista de empleados a la vista, que está almacenada en la lista estática
            return View(empleados); // Usamos la lista de empleados que está en la clase
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
