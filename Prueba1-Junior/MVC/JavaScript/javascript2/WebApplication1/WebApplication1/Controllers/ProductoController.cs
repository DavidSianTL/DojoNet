using Microsoft.AspNetCore.Mvc;
using pruebaJs2.Models;

namespace pruebaJs2.Controllers;

public class ProductoController : Controller
{
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Producto producto)
    {
        return View("Confirmacion", producto);
    }
}
