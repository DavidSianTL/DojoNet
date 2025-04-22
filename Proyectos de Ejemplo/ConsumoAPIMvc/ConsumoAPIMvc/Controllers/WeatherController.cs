using Microsoft.AspNetCore.Mvc;

namespace ConsumoAPIMvc.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
