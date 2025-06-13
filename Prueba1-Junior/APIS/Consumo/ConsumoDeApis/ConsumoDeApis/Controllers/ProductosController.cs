using Microsoft.AspNetCore.Mvc;

namespace ConsumoDeApis.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Productos()
        {
            return View();
        }
    }
}
