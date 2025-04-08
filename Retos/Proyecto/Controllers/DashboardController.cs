using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace Proyecto.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Usuario = usuario;
            return View();
        }
    }
}
