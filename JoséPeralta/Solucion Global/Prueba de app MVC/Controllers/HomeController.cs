using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prueba_de_app_MVC.Models;

namespace Prueba_de_app_MVC.Controllers
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

        public IActionResult RegistroDePuestos()
        {
            return View();
        }

        public IActionResult RegistroParaVacaciones()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistroPuestoEmpleado(PuestoEmpleadoModel puestoEmpleado)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ViewBag.Mensaje = "Registro exitoso";
                    return View("RegistroDePuestos", puestoEmpleado);
                }
                else
                {
                    ViewBag.Mensaje = "Error, vuelve a intentarlo";
                    return View("RegistroDePuestos", puestoEmpleado);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult RegistroVacacionesEmpleado(PeticionVacacionesModel vacaciones)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Mensaje = "Solicitud de vacaciones exitosa";
                    return View("RegistroParaVacaciones", vacaciones);
                }
                else
                {
                    ViewBag.Mensaje = "Error, vuelve a intentarlo";
                    return View("RegistroParaVacaciones", vacaciones);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return View("Error");
            }
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
