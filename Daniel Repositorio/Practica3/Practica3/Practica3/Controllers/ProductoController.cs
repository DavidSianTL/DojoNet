using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practica3.Models;

namespace Practica3.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio _servicio;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger)
        {
            _servicio = new ProductoServicio();
            _logger = logger;
        }

        public IActionResult Index()
        {
            var productos = _servicio.ObtenerTodos();
            _logger.LogInformation("Se accedió a la vista de productos. Total: {Cantidad}", productos.Count);

            // Verifica si hay mensajes en la sesión para mostrar SweetAlert
            if (HttpContext.Session.GetString("Mensaje") != null)
            {
                ViewBag.Mensaje = HttpContext.Session.GetString("Mensaje");
                ViewBag.Tipo = HttpContext.Session.GetString("Tipo");
                HttpContext.Session.Remove("Mensaje");
                HttpContext.Session.Remove("Tipo");
            }

            return View(productos);
        }

        public IActionResult Crear()
        {
            _logger.LogInformation("Se accedió a la vista para crear un nuevo producto.");
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _servicio.Agregar(producto);
                _logger.LogInformation("Se creó un nuevo producto: {Nombre} - Precio: {Precio} - Stock: {Stock}",
                    producto.Nombre, producto.Precio, producto.Stock);

                // Guardamos mensaje en sesión
                HttpContext.Session.SetString("Mensaje", "Producto agregado correctamente.");
                HttpContext.Session.SetString("Tipo", "exito");

                return RedirectToAction("Index");
            }

            _logger.LogWarning("Error de validación al intentar crear producto: {@Producto}", producto);
            ViewBag.Mensaje = "Error al guardar el producto.";
            ViewBag.Tipo = "error";
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _servicio.ActualizarProducto(producto);
                _logger.LogInformation("Se actualizó el producto ID {Id}: {Nombre} - Precio: {Precio} - Stock: {Stock}",
                    producto.Id, producto.Nombre, producto.Precio, producto.Stock);

                HttpContext.Session.SetString("Mensaje", "Producto actualizado correctamente.");
                HttpContext.Session.SetString("Tipo", "exito");

                return RedirectToAction("Index");
            }

            _logger.LogWarning("Error de validación al intentar editar producto: {@Producto}", producto);
            ViewBag.Mensaje = "Error al actualizar el producto.";
            ViewBag.Tipo = "error";
            return View(producto);
        }

        public IActionResult Eliminar(int id)
        {
            _servicio.Eliminar(id);
            _logger.LogInformation("Se eliminó el producto con ID: {Id}", id);

            HttpContext.Session.SetString("Mensaje", "Producto eliminado correctamente.");
            HttpContext.Session.SetString("Tipo", "exito");

            return RedirectToAction("Index");
        }
    }
}
