using Microsoft.AspNetCore.Mvc;
using Examen_mes_abril.Models;
using Examen_mes_abril.Services;

namespace Examen_mes_abril.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicioModel _productoService;
        public ProductoController(ProductoServicioModel productoService)
        {
            _productoService = productoService;
        }

        //Vista para crear un producto
        public IActionResult Crear()
        {
            BitacoraService.RegistrarEvento("Operacion CRUD", $"Se accedió a la vista de Crear");
            return View();
        }

        //Acción específica para crear un producto
        [HttpPost]
        public IActionResult Crear(ProductoViewModel producto)
        {
            if (ModelState.IsValid)
            {
                _productoService.GuardarProducto(producto);
                TempData["ProductoSuccess"] = "El producto se ha agregado correctamente";
                BitacoraService.RegistrarEvento("Operacion CRUD", $"El producto se ha agregado correctamente");
                return RedirectToAction("Ver", "Producto");
            }
            return View(producto);
        }
        // Acción para mostrar la lista de productos

        public IActionResult Ver()
        {
            var productos = _productoService.ObtenerProductos();
            BitacoraService.RegistrarEvento("Operacion CRUD", $"Se accedió a la vista Ver productos");
            return View(productos);
        }

        //Acción para buscar verificar si existe el producto por Id
        public IActionResult Editar(int id)
        {

            var producto = _productoService.ObtenerProductoPorId(id);
            if (producto == null)
            {
                BitacoraService.RegistrarEvento("Operacion CRUD", $"El producto no existe");
                return NotFound();
            }
            return View(producto);
        }

        //Acción para buscar actualizar el producto 
        [HttpPost]
        public IActionResult Editar(ProductoViewModel producto)
        {

            if (ModelState.IsValid)
            {
                _productoService.ActualizarProducto(producto);
                TempData["ProductoSuccess"] = "El producto se ha editado correctamente";
                BitacoraService.RegistrarEvento("Operacion CRUD", $"El producto se editó correctamente");
                return RedirectToAction("Ver");
            }
            return View("~/Views/Producto/Ver.cshtml", producto);
        }

        // Mostrar la vista de confirmación
        public IActionResult Eliminar(int id)
        {
            var producto = _productoService.ObtenerProductoPorId(id);
            if (producto == null)
            {
                BitacoraService.RegistrarEvento("Operacion CRUD", $"El producto no existe");
                return NotFound();
            }

            return View(producto);
        }

        // Procesar la eliminación
        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            _productoService.EliminarProducto(id);
            TempData["ProductoSuccess"] = "El producto se ha eliminado correctamente";
            BitacoraService.RegistrarEvento("Operacion CRUD", $"El producto se ha eliminado correctamente");
            return RedirectToAction("Ver");
        }

    }
}

