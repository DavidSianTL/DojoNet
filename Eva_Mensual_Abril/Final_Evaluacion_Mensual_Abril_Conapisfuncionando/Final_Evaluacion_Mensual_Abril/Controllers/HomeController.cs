using Microsoft.AspNetCore.Mvc;

using Final_Evaluacion_Mensual_Abril.Models;
using System.Diagnostics;

namespace Evaluacion_Mensual_Abril.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login");
            }

            
            return RedirectToAction("Index", "Producto");
        }

        public IActionResult Privacy()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Login", "Login");
            }

            ConfigurarViewBag();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        private bool UsuarioAutenticado()
        {
            return HttpContext.Session.GetString("UsrNombre") != null;
        }

        private void ConfigurarViewBag()
        {
            ViewBag.UsrNombre = HttpContext.Session.GetString("UsrNombre");
            ViewBag.NombreCompleto = HttpContext.Session.GetString("NombreCompleto");
        }
    }
}