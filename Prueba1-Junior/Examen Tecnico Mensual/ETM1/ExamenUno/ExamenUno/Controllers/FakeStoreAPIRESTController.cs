using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;
using ExamenUno.Models;

namespace ExamenUno.Controllers
{
    public class FakeStoreAPIRESTController : Controller
    {
        public IFakeStoreAPIService _client;


        public FakeStoreAPIRESTController(IFakeStoreAPIService client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ShowProd()
        {
            var products = await _client.ShowProdAsync();

            return View(products);
        }
    }
}
