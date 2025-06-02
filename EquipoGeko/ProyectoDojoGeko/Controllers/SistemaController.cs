using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class SistemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
