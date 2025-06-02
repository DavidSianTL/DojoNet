using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class BitacoraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
