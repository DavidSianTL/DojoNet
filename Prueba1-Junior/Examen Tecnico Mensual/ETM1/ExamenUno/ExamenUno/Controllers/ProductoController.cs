using System.Text.Json;
using ExamenUno.Models;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class ProductoController : Controller
	{
		private readonly ISessionService _sessionService;


		
		private  List<Producto> readProducts()
		{
			string productsRoute = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
			var content = System.IO.File.ReadAllText(productsRoute);
			var products = JsonSerializer.Deserialize<List<Producto>>(content);

			return products;
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
