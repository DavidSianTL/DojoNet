using Microsoft.AspNetCore.Mvc;

namespace MiPrimeraAppMVC.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
