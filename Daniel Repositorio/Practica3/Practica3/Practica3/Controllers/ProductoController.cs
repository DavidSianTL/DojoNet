using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practica3.Models;

namespace Practica3.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio _servicio;
        private readonly ILogger<ProductoController> _logger;

        // Constructor del controlador. Se inicializa el servicio de productos y el logger.
        public ProductoController(ILogger<ProductoController> logger)
        {
            _servicio = new ProductoServicio(); // Servicio que gestiona los productos
            _logger = logger; // Logger para registrar información de acciones realizadas
        }

        // Acción que muestra todos los productos en la vista principal
        public IActionResult Index()
        {
            var productos = _servicio.ObtenerTodos(); // Obtiene la lista de productos
            _logger.LogInformation("Se accedió a la vista de productos. Total: {Cantidad}", productos.Count);
            return View(productos);
        }

        // Acción GET que muestra el formulario para crear un nuevo producto
        public IActionResult Crear()
        {
            _logger.LogInformation("Se accedió a la vista para crear un nuevo producto.");
            return View();
        }

        // Acción POST que recibe los datos del nuevo producto y lo guarda
        [HttpPost]
        public IActionResult Crear(Producto producto)
        {
            // Verifica que los datos enviados sean válidos según el modelo
            if (ModelState.IsValid)
            {
                _servicio.Agregar(producto); // Agrega el nuevo producto
                _logger.LogInformation("Se creó un nuevo producto: {Nombre} - Precio: {Precio} - Stock: {Stock}",
                    producto.Nombre, producto.Precio, producto.Stock);
                return RedirectToAction("Index"); // Redirige a la lista de productos
            }

            // Si hay errores en el modelo, se registra advertencia y se vuelve a mostrar el formulario
            _logger.LogWarning("Error de validación al intentar crear producto: {@Producto}", producto);
            return View(producto);
        }

        // Acción POST que actualiza los datos de un producto existente
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _servicio.ActualizarProducto(producto); // Actualiza la información del producto
                _logger.LogInformation("Se actualizó el producto ID {Id}: {Nombre} - Precio: {Precio} - Stock: {Stock}",
                    producto.Id, producto.Nombre, producto.Precio, producto.Stock);
                return RedirectToAction("Index");
            }

            // Si hay errores en el modelo, se registra advertencia
            _logger.LogWarning("Error de validación al intentar editar producto: {@Producto}", producto);
            return View(producto);
        }

        // Acción que elimina un producto por su ID
        public IActionResult Eliminar(int id)
        {
            _servicio.Eliminar(id); // Elimina el producto del sistema
            _logger.LogInformation("Se eliminó el producto con ID: {Id}", id);
            return RedirectToAction("Index");
        }
    }
}

