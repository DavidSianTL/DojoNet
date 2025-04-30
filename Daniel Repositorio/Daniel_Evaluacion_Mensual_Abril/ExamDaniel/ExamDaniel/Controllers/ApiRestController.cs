using Microsoft.AspNetCore.Mvc;
using ExamDaniel.Models;
using ExamDaniel.Servicios;
using ExamDaniel.bitacora;

namespace ExamDaniel.Controllers
{
    public class ApiRestController : Controller
    {
        private readonly ApiRestService _apiService;

        public ApiRestController(ApiRestService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _apiService.ObtenerProductosAsync();
            return View(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ProductoApiRest modelo)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Los datos del producto no son válidos.";
                return RedirectToAction("Index");
            }

            try
            {
                var creado = await _apiService.CrearProductoAsync(modelo);
                TempData["Success"] = "Producto creado correctamente.";
                BitacoraManager.RegistrarEvento("REST", $"Crear producto: {creado.Id}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear el producto.";
                BitacoraManager.RegistrarEvento("Error", $"Crear producto: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ProductoApiRest modelo)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Los datos del producto no son válidos.";
                return RedirectToAction("Index");
            }

            try
            {
                var editado = await _apiService.EditarProductoAsync(modelo.Id, modelo);
                TempData["Success"] = "Producto editado correctamente.";
                BitacoraManager.RegistrarEvento("REST", $"Editar producto: {editado.Id}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al editar el producto.";
                BitacoraManager.RegistrarEvento("Error", $"Editar producto: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _apiService.EliminarProductoAsync(id);
                TempData["Success"] = "Producto eliminado correctamente.";
                BitacoraManager.RegistrarEvento("REST", $"Eliminar producto: {eliminado.Id}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar el producto.";
                BitacoraManager.RegistrarEvento("Error", $"Eliminar producto: {ex.Message}");
            }

            return RedirectToAction("Index");
        }
    }
}
