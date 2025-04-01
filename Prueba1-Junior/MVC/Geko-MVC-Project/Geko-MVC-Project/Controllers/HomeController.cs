using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Geko_MVC_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geko_MVC_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(UsuarioModel usuarioModel)

        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Mensaje = "exito";
                    return View("index", usuarioModel);
                }
                else
                {
                    ViewBag.Mensaje = "malo"; }
                return View("index", usuarioModel|);
                }
            }
            catch
            {
                logger.LogError(Exception, Exception.Message)
                ViewBag.Mensaje = "Ocurrio un error inesperado";
                Return view("index", usuarioModel);
        }
        }



        [HttpPost]
        public IActionResult Registro()
        {

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
