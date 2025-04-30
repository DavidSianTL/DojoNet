
using Microsoft.AspNetCore.Mvc;
using Evaluacion_Mensual_Abril.Models;

namespace Evaluacion_Mensual_Abril.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoServicio _productoServicio;

        public ProductoController(ProductoServicio productoServicio)
        {
            _productoServicio = productoServicio;
        }

        public IActionResult Index()
        {
            var productos = _productoServicio.ObtenerTodos();
            return View(productos);
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
                _productoServicio.Agregar(producto);
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        public IActionResult Editar(int id)
        {
            var producto = _productoServicio.ObtenerPorId(id);
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
                _productoServicio.Actualizar(producto);
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        public IActionResult Eliminar(int id)
        {
            var producto = _productoServicio.ObtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            _productoServicio.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}