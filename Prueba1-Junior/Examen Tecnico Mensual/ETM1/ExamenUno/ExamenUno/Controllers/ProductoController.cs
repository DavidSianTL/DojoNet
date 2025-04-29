using System.Text.Json;
using ExamenUno.Models;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class ProductoController : Controller
	{
		private readonly ISessionService _sessionService;


		
			string productsRoute = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
		private  List<Producto> readProducts()
		{
			var content = System.IO.File.ReadAllText(productsRoute);
			var products = JsonSerializer.Deserialize<List<Producto>>(content);

			return products;
		}
		private void saveProduct(Producto product)
		{
			var products = readProducts();
			
			//generamos un id
			int newId = products.Any() ? products.Max(p  => p.Id) +1 : 1;
			product.Id = newId;//asignamos el id

			products.Add(product);
			
			var content = JsonSerializer.Serialize(products, new JsonSerializerOptions{ WriteIndented = true });
			System.IO.File.WriteAllText(productsRoute, content);
		}

		public ProductoController( ISessionService sessionService)
		{
			_sessionService = sessionService;
		}




		[HttpGet]
		public IActionResult ShowProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;
			var producs = readProducts();


			return View(producs);
		}

		[HttpGet]
		public IActionResult CreateProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			return View();
		}



		[HttpPost]
		public IActionResult CreateProd(Producto product)
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;



			if (!ModelState.IsValid)
			{
				return View();
			}
			if (string.IsNullOrEmpty(product.productName) || decimal.IsNegative(product.price))
			{
				return View();
			}


			try
			{
				 saveProduct(product);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "Error al guardar el producto");
				return View();
			}

			return RedirectToAction("ShowProd", "Producto");
		}

		[HttpGet]
		public IActionResult EditProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			return View();
		}

		[HttpGet]
		public IActionResult DeleteProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			return View();
		}

		

	}
}
