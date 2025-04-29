using Microsoft.AspNetCore.Mvc;
using ExamDaniel.Models;
using ExamDaniel.bitacora;

namespace ExamDaniel.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio _servicio = new();

        public IActionResult Index()
        {
            var productos = _servicio.ObtenerTodos();
            return View(productos);
        }

        [HttpPost]
        public IActionResult Agregar(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _servicio.Agregar(producto);
                    BitacoraManager.RegistrarEvento("Operación", $"Producto agregado: {producto.Nombre} (Stock: {producto.Stock}, Precio: {producto.Precio})");
                }
                else
                {
                    BitacoraManager.RegistrarEvento("Error", "Error al agregar un producto. Modelo no válido.");
                }
            }
            catch (Exception ex)
            {
                BitacoraManager.RegistrarEvento("Error", $"Excepción al agregar producto: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _servicio.Editar(producto);
                    BitacoraManager.RegistrarEvento("Operación", $"Producto editado: ID {producto.Id} (Nuevo nombre: {producto.Nombre})");
                }
                else
                {
                    BitacoraManager.RegistrarEvento("Error", $"Error al editar producto ID {producto.Id}. Modelo no válido.");
                }
            }
            catch (Exception ex)
            {
                BitacoraManager.RegistrarEvento("Error", $"Excepción al editar producto ID {producto.Id}: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _servicio.Eliminar(id);
                BitacoraManager.RegistrarEvento("Operación", $"Producto eliminado: ID {id}");
            }
            catch (Exception ex)
            {
                BitacoraManager.RegistrarEvento("Error", $"Excepción al eliminar producto ID {id}: {ex.Message}");
            }

            return RedirectToAction("Index");
        }
    }
}
