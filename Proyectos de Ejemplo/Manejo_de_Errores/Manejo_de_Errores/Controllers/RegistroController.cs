using Microsoft.AspNetCore.Mvc;

namespace Manejo_de_Errores.Controllers
{
    public class RegistroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
