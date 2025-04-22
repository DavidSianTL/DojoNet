using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class WeaponsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
