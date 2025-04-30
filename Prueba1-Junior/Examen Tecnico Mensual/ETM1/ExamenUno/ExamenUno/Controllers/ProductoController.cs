using System.Runtime.CompilerServices;
using System.Text.Json;
using ExamenUno.Models;
using ExamenUno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
	public class ProductoController : Controller
	{
		private readonly ILogger<ProductoController> _logger;
		public ProductoController( ILogger<ProductoController> logger , ISessionService sessionService)
		{
			_logger = logger;
			_sessionService = sessionService;
		}


		private readonly ISessionService _sessionService;
		
		string productsRoute = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
		private  List<Producto> readProducts()
		{
			if(!System.IO.File.Exists(productsRoute)) return new List<Producto>();
			try
			{

				var content = System.IO.File.ReadAllText(productsRoute);
				var products = JsonSerializer.Deserialize<List<Producto>>(content);

				return products;
			}
			catch(Exception ex){
				LoggerService.LogError(ex);
				return new List<Producto>();

			}

		}


		private string saveProduct(List<Producto> products)
		{
			try
			{
                var content = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(productsRoute, content);
                return "Producto guardado exitosamente";
			}
			catch (Exception ex){
				LoggerService.LogError(ex);
				return "Error al guardar el archivo";
			}
		}



		[HttpGet]
		public IActionResult ShowProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;
			try
			{	
				var user = HttpContext.Session.GetString("User");
				_logger.LogInformation($"El usuario: {user} accedió a la pagina de mostrar productos");

                var products = readProducts();
				return View(products);

            }
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
                return RedirectToAction("Index", "Home");
            }
		}



		[HttpGet]
		public IActionResult CreateProd()
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina de Crear productos");
            return View();
		}

		[HttpPost]
		public IActionResult CreateProd(Producto product)
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			//validaciones de modelo y campos
			if (!ModelState.IsValid) return View();
			if (string.IsNullOrEmpty(product.productName) || decimal.IsNegative(product.price)) return View();

			try
			{
				var products = readProducts();

				//generamos un id
				int newId = products.Any() ? products.Max(p => p.Id) + 1 : 1;
				product.Id = newId;//asignamos el id

				products.Add(product);
				saveProduct(products);
				_logger.LogInformation($"El usuario {HttpContext.Session.GetString("User")} a creado el producto {product.productName} | Hora: {DateTime.Now}");
			}

			catch (Exception ex){

				_logger.LogCritical($"Error al crear el producto {product.productName} El usuario {HttpContext.Session.GetString("User")}  | Hora: {DateTime.Now}");
				LoggerService.LogError(ex);
				ModelState.AddModelError(string.Empty, "Error al guardar el producto");
				return View(product);
			}

			return RedirectToAction("ShowProd", "Producto");
		}




		[HttpGet]
		public IActionResult EditProd(int id)
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

            _logger.LogInformation($"El usuario: {HttpContext.Session.GetString("User")} accedió a la pagina de Editar productos");

            var products = readProducts();
			var validProduct = products.FirstOrDefault(p => p.Id == id);

			if (validProduct == null) return NotFound();

			return View(validProduct);
		}

		[HttpPost]
		public IActionResult EditProd(Producto product)
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;


			if (!ModelState.IsValid) return View();
			if(string.IsNullOrEmpty(product.productName) || decimal.IsNegative(product.price)) return View(product);

			try
			{
                var products = readProducts();
                var index = products.FindIndex(p => p.Id == product.Id);

                if (index <= -1) return NotFound();

                products[index] = product;

                saveProduct(products);

                _logger.LogInformation($"El usuario {HttpContext.Session.GetString("User")} a Editado el producto {product.productName} | Hora: {DateTime.Now}");
                return RedirectToAction("ShowProd");
			}
			catch (Exception ex){
				LoggerService.LogError(ex);
				return View(product);
			}
		}



		[HttpGet]
		public IActionResult DeleteProd(int id)
		{
			var redirect = _sessionService.validateSession(HttpContext);
			if (redirect != null) return redirect;

			try
			{

				var products = readProducts();

				var validProduct = products.FirstOrDefault(p => p.Id == id);
				if (validProduct == null) return RedirectToAction("ShowProd");

				products.Remove(validProduct);
				saveProduct(products);

                _logger.LogInformation($"El usuario {HttpContext.Session.GetString("User")} a Eliminado el producto {validProduct.productName} | Hora: {DateTime.Now}");
                return RedirectToAction("ShowProd");

			}
			catch (Exception ex)
			{
				LoggerService.LogError(ex);
				ModelState.AddModelError(string.Empty, "Error al eliminar el producto");
				return RedirectToAction("ShowProd");
			}
		}

		

	}
}
