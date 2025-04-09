using Microsoft.AspNetCore.Mvc;
using Practica3.Models;

namespace Practica3.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio _servicio;

        public ProductoController()
        {
            _servicio = new ProductoServicio();
        }

        // Visualizar tabla productos
        public IActionResult Index()
        {
            var productos = _servicio.ObtenerTodos();
            return View(productos);
        }

        // Crear producto
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _servicio.Agregar(producto);
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // EDITAR producto
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _servicio.ActualizarProducto(producto);
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // Eliminar producto
        public IActionResult Eliminar(int id)
        {
            _servicio.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
