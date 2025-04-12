using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Proyecto.Utils; // Para usar Logger

namespace Proyecto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string SessionUserId = "UsuarioId";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetString(SessionUserId);
            if (!string.IsNullOrEmpty(usuarioId))
            {
                Logger.RegistrarAccion(usuarioId, "Ingresó a la página de inicio");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            var usuarioId = HttpContext.Session.GetString(SessionUserId);
            if (!string.IsNullOrEmpty(usuarioId))
            {
                Logger.RegistrarAccion(usuarioId, "Visitó la página de privacidad");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var usuarioId = HttpContext.Session.GetString(SessionUserId);
            if (!string.IsNullOrEmpty(usuarioId))
            {
                Logger.RegistrarAccion(usuarioId, "Accedió a la vista de error");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
