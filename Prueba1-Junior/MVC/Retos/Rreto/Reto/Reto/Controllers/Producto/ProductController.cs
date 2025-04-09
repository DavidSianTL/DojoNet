using Microsoft.AspNetCore.Mvc;

namespace Reto.Controllers.Producto
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
