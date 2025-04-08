using Microsoft.AspNetCore.Mvc;

namespace MiPrimeraAppMVC.Controllers
{
    public class Usuarios : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
