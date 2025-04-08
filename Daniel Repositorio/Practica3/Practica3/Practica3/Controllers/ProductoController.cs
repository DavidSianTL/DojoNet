using Microsoft.AspNetCore.Mvc;
using Practica3.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Practica3.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio _servicio;

        public ProductoController()
        {
            _servicio = new ProductoServicio();
        }

        //Visualizar tabla productos
        public IActionResult Index()
        {
            var productos = _servicio.ObtenerTodos();
            return View(productos);
        }

        //Crear producto
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

        //Editar producto
        public IActionResult Edit(int id)
        {
            var producto = _servicio.ObtenerProductoPorId(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        // UPDATE - Guardar cambios
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

        // DELETE - Eliminar producto
        public IActionResult Eliminar(int id)
        {
            _servicio.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
