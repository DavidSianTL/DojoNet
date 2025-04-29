using Microsoft.AspNetCore.Mvc;
using ExamDaniel.Models;

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
            if (ModelState.IsValid)
            {
                _servicio.Agregar(producto);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _servicio.Editar(producto);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            _servicio.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
