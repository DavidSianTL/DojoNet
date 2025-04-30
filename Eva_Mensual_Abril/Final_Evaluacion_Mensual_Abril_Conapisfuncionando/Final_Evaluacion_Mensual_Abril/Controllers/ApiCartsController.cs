using Final_Evaluacion_Mensual_Abril.Models;
using Final_Evaluacion_Mensual_Abril.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Evaluacion_Mensual_Abril.Controllers
{
    public class ApiCartsController : Controller
    {
        private readonly FakeStoreService _fakeStoreService;

        public ApiCartsController(FakeStoreService fakeStoreService)
        {
            _fakeStoreService = fakeStoreService;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _fakeStoreService.GetCartsAsync();
            return View(carts);
        }

        
    }
}