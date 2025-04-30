using Final_Evaluacion_Mensual_Abril.Models;
using Final_Evaluacion_Mensual_Abril.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Evaluacion_Mensual_Abril.Controllers
{
    public class ApiStoreController : Controller
    {
        private readonly FakeStoreService _fakeStoreService;

        public ApiStoreController(FakeStoreService fakeStoreService)
        {
            _fakeStoreService = fakeStoreService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _fakeStoreService.GetProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _fakeStoreService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}