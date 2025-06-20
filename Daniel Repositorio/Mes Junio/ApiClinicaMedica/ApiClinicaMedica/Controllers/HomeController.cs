using Microsoft.AspNetCore.Mvc;

namespace ApiClinicaMedica.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
