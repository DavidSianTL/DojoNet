using Microsoft.AspNetCore.Mvc;

namespace Manejo_de_Errores.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
