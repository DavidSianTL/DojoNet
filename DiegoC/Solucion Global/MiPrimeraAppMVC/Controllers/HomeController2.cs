using Microsoft.AspNetCore.Mvc;

namespace MiPrimeraAppMVC.Controllers
{
    public class HomeController2 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
