
using Microsoft.AspNetCore.Mvc;
using pruebas.Models;
using System.Collections.Generic;
using System.Linq;

public class ProductoController : Controller
{
    // Lista estática que simula una base de datos en memoria
    private static List<producto> _productos = new List<producto>();

    // Mostrar la lista de productos
    public IActionResult Lista()
    {
        return View(_productos);
    }

    // Vista para crear un producto (GET)
    public IActionResult Crear()
    {
        return View();
    }

    // Recibir datos del formulario de creación (POST)
    [HttpPost]
    public IActionResult Crear(producto producto)
    {
        if (ModelState.IsValid)
        {
            _productos.Add(producto);  // Agrega el producto a la lista
            return RedirectToAction("Lista");  // Redirige a la vista de lista
        }

        // Si hay errores en los datos, vuelve al formulario
        return View(producto);
    }

    // Vista para editar un producto existente (GET)
    public IActionResult Editar(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    // Guardar cambios al editar un producto (POST)
    [HttpPost]
    public IActionResult Editar(producto producto)
    {
        if (ModelState.IsValid)
        {
            var prod = _productos.FirstOrDefault(p => p.Id == producto.Id);
            if (prod != null)
            {
                prod.Nombre = producto.Nombre;
                prod.Tipo = producto.Tipo;
                prod.Consola = producto.Consola;
                prod.Precio = producto.Precio;
            }
            return RedirectToAction("Lista");
        }
        return View(producto);
    }

    // Eliminar un producto
    public IActionResult Eliminar(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);
        }
        return RedirectToAction("Lista");
    }
}

