using Microsoft.AspNetCore.Mvc;

namespace SistemaAutenticacion.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
