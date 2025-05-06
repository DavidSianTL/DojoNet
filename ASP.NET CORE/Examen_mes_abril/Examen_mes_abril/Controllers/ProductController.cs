using Microsoft.AspNetCore.Mvc;
using Examen_mes_abril.Models;
using Examen_mes_abril.Services;
using System.Threading.Tasks;

namespace Examen_mes_abril.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _service = new ProductService();

        // Accion para mostrar los productos
        public async Task<ActionResult> Index()
        {
            var product = await _service.GetAllProductsAsync();
            
            BitacoraService.RegistrarEvento("Operacion REST", $"Se accedió a la vista Index de productos");
            return View(product);
        }

        // Accion para mostrar los detalles de un producto
        public async Task<ActionResult> Details(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            BitacoraService.RegistrarEvento("Operacion REST", $"Se accedió a la vista de Details productos");
            return View(product);
        }

        public ActionResult Create()
        {
            BitacoraService.RegistrarEvento("Operacion REST", $"Se accedió a la vista Create de productos");
            return View();
        }

        // Accion para crear productos
        [HttpPost]
        public async Task<ActionResult> Create(ProductModel product)
        {
            await _service.CreateProductAsync(product);
            TempData["ProductoSuccess"] = "El producto se ha creado correctamente";
            BitacoraService.RegistrarEvento("Operacion REST", $"Se creo el producto correctamente");
            return RedirectToAction("Index");
        }

        // Accion para editar productos
        public async Task<ActionResult> Edit(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ProductModel product)
        {
            await _service.UpdateProductAsync(id, product);
            TempData["ProductoSuccess"] = "El producto se ha editado correctamente";
            BitacoraService.RegistrarEvento("Operacion REST", $"Se editó el producto correctamente");
            return RedirectToAction("Index");
        }

        // Accion para eliminar productos
        public async Task<ActionResult> Delete(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _service.DeleteProductAsync(id);
            TempData["ProductoSuccess"] = "El producto se ha eliminado correctamente";
            BitacoraService.RegistrarEvento("Operacion REST", $"Se eliminó el producto correctamente");
            return RedirectToAction("Index");
        }
    }
}

