using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;
using ExamenUno.Models;

namespace ExamenUno.Controllers
{
    public class FakeStoreAPIRESTController : Controller
    {
        public IFakeStoreAPIService _client;
        private readonly ISessionService _sessionService;

        public FakeStoreAPIRESTController(IFakeStoreAPIService client, ISessionService sessionService)
        {
            _client = client;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> ShowProduct()
        {
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            var products = await _client.ShowProdAsync();
            
            return View(products);
        }

        [HttpGet]
        public  IActionResult CreateProduct()
        {
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(FakeProduct product)
        {
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            var response = await _client.CreateProdAsync(product);
            TempData["status"] = $"el producto {response.description} ah sido guardado con exito";
            return RedirectToAction("ShowProduct");
        }


    }
}
