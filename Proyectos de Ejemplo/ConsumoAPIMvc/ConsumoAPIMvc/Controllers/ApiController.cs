using Microsoft.AspNetCore.Mvc;

namespace ConsumoAPIMvc.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
