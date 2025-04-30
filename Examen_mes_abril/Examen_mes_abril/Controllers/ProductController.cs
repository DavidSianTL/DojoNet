using Microsoft.AspNetCore.Mvc;
using Examen_mes_abril.Models;
using Examen_mes_abril.Services;
using System.Threading.Tasks;

namespace Examen_mes_abril.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _service = new ProductService();

        public async Task<ActionResult> Index()
        {
            var product = await _service.GetAllProductsAsync();
            return View(product);
        }

        public async Task<ActionResult> Details(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return View(product);
        }

        public ActionResult Create() => View();

        [HttpPost]
        public async Task<ActionResult> Create(ProductModel product)
        {
            await _service.CreateDeviceAsync(product);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ProductModel product)
        {
            await _service.UpdateProductAsync(id, product);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _service.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }
    }
}

