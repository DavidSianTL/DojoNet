using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
