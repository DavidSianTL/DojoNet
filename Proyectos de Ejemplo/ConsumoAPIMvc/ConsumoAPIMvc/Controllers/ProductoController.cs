using Microsoft.AspNetCore.Mvc;

namespace ConsumoAPIMvc.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
