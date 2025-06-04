using Microsoft.AspNetCore.Mvc;
using Solucion_Reto_3_MVC.Models;

namespace Solucion_Reto_3_MVC.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult VerVista()
        {
            return View("~/Views/Producto/Producto.cshtml");
        }
    }
}
