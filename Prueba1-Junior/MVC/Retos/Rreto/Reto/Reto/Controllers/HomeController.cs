using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Reto.Models;

namespace Reto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IActionResult Index()
        {
            string usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario)) return RedirectToAction("Login", "Login");

            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
