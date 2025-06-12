using Microsoft.AspNetCore.Mvc;

namespace SistemaDeSeguridad.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
