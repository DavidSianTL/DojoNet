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
        public async Task<IActionResult> ShowProd()
        {
            var products = await _client.ShowProdAsync();

            return View(products);
        }

        [HttpGet]
        public  IActionResult CreateProd()
        {
            var redirect = _sessionService.validateSession(HttpContext);
            if (redirect != null) return redirect;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProd(FakeProduct product)
        {
            var response = await _client.CreateProdAsync(product);

            return RedirectToAction("ShowProd");
        }

    }
}
