using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practica3.Models;

namespace Practica3.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            //verificamos si el usuario esta logeado
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login");


            }
            ViewBag.usrNombre = usrNombre;
            ViewBag.NombreCompleto = NombreCompleto;



            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
