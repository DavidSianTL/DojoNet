using EjemploConsumirApiRest.Models.DummyJSONModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EjemploConsumirApiRest.Controllers.DummyJSONController
{
    public class ProductsController : Controller
    {
        private readonly ProductsService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductsService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View("~/Views/DummyJSON/Index.cshtml", products);
        }

        public IActionResult CreateView()
        {
            return View("~/Views/DummyJSON/Create.cshtml");
        }

        public async Task<IActionResult> UpdateView(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            _logger.LogInformation("Contenido de product: {Product}", JsonSerializer.Serialize(product));
            if (product == null)
            {
                return NotFound();
            }
            return View("~/Views/DummyJSON/Update.cshtml", product);
        }

        // POST: Products/Create
        // Creamos el método para crear un producto
        // Pasamos el modelo de producto como parámetro
        // Y lo convertimos a JSON para enviarlo en el cuerpo de la solicitud
        //
        [HttpPost]
        public async Task<IActionResult> Create(ProductsViewModel product)
        {
            if (ModelState.IsValid)
            {
                var createdProduct = await _productService.AddProductAsync(product);
                if (createdProduct != null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("~/Views/DummyJSON/Create.cshtml", product);
        }

        // PUT: Products/Update
        [HttpPut]
        public async Task<IActionResult> Update(int idProducto, ProductsViewModel product)
        {

            // Quemamos el id del producto
            idProducto = 1;

            // Verificamos si el modelo es válido
            // Si no es válido, devolvemos la vista con el modelo
            if (ModelState.IsValid)
            {
                var updatedProduct = await _productService.UpdatedProductAsync(idProducto, product);
                if (updatedProduct != null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("~/Views/DummyJSON/Update.cshtml", product);
        }


    }
}
