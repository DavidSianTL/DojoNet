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
			try
			{
				var response = await _client.CreateProdAsync(product);
				TempData["status"] = $"el producto {response.description} ah sido guardado con exito";
				return RedirectToAction("ShowProduct");

			}catch(Exception ex){

				TempData["status"] = $"El producto no se pudo editar intentelo más tarde";
				LoggerService.LogError(ex);
				return RedirectToAction("ShowProduct");

			}

		}

		[HttpGet]
		public async Task<IActionResult> EditProduct(int id)
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			try
			{
				var products = await _client.ShowProdAsync();
				var validProduct = products.FirstOrDefault(p=> p.id == id);
				if (validProduct == null) return NotFound();

				return View(validProduct);

			}catch (Exception ex)
			{
				LoggerService.LogError(ex);
				return NotFound();
			}

		}
		[HttpPost]
		public async Task<IActionResult> EditProduct(int id,FakeProduct product)
		{
			try
			{
				var result = await _client.EditProdAsync(id, product);
				if (result == null) return NotFound();

				TempData["status"] = $"El producto {result.title} se ha actualizado correctamente";
				return RedirectToAction("ShowProduct");
			}
			catch(Exception ex){

				LoggerService.LogError(ex);
				return View(product);

			}
		}


	}
}
