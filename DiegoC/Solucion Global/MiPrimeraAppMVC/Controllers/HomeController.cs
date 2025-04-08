using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAppMVC.Models;

namespace MiPrimeraAppMVC.Controllers
{

    public class HomeController : Controller
    {
        private static List<SolicitudVacacionesModel> _solicitudes = new List<SolicitudVacacionesModel>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost] // Se agrega este atributo para indicar que el método es de tipo POST
        //Preguntar poruqe no me dejaba llenar mas de 14 caracteres
        public IActionResult Registro(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Mensaje = "Usuario registrado con éxito.";
                    return View("Index", usuario);
                }
                else
                {
                    ViewBag.Mensaje = "Error: Algunos campos no son válidos.";
                    return View("Index", usuario); // Se asegura de retornar la vista correcta
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Registro: {ex.Message}");
                ViewBag.Mensaje = "Ocurrió un error inesperado.";
                return View("Index", usuario);
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
        public IActionResult Crear()
        {
            return View();
        }
       public IActionResult Versolicitudes()
        {
            return View();
        }
    }
}
