using Microsoft.AspNetCore.Mvc;
using Proyercto1.Models;
using Proyercto1.Filters;

[LoginAuthorize] 
public class ProductoController : Controller
{
    private static List<Producto> _productos = new List<Producto>();

    public IActionResult Dashboard()
    {
        return View(_productos);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Producto producto)
    {
        if (ModelState.IsValid)
        {
            _productos.Add(producto);
            return RedirectToAction("Dashboard");
        }
        return View(producto);
    }

    public IActionResult Editar(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult Editar(Producto producto)
    {
        if (ModelState.IsValid)
        {
            var prod = _productos.FirstOrDefault(p => p.Id == producto.Id);
            if (prod != null)
            {
                prod.Nombre = producto.Nombre;
                prod.Precio = producto.Precio;
                prod.Descripcion = producto.Descripcion;
                prod.Stock = producto.Stock;
            }
            return RedirectToAction("Dashboard");
        }
        return View(producto);
    }

    public IActionResult Eliminar(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);
        }
        return RedirectToAction("Dashboard");
    }
}
