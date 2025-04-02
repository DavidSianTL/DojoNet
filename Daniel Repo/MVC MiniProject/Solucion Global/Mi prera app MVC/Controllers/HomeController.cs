using System.Diagnostics;
using Mi_prera_app_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mi_prera_app_MVC.Controllers  
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

        public IActionResult Registro(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Mensaje = "Usuario registrado correctamente.";
                    return View("Index", usuario);
                }
                else
                {
                    ViewBag.Mensaje = "Error: algunos campos no han sido validos.";
                    return View("Index", usuario);// Se asegura de retornar la vista correcta

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en Registro: {ex.Message}");
                ViewBag.Mensaje = "Ocurrio un error inesperado";
                return RedirectToAction("Index");   
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
