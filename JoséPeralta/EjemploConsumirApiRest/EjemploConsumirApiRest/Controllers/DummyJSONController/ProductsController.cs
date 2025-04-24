using EjemploConsumirApiRest.Models.DummyJSONModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EjemploConsumirApiRest.Controllers.DummyJSONController
{
    public class ProductsController : Controller
    {
        private readonly ProductsService _productService;

        public ProductsController(ProductsService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View("~/Views/DummyJSON/Index.cshtml", products);
        }

        public async Task<IActionResult> CreateView()
        {
            return View("~/Views/DummyJSON/Create.cshtml");
        }

        public async Task<IActionResult> UpdateView()
        {
            return View("~/Views/DummyJSON/Update.cshtml");
        }

        // POST: Products/Create
        // Creamos el método para crear un producto
        // Pasamos el modelo de producto como parámetro
        // Y lo convertimos a JSON para enviarlo en el cuerpo de la solicitud
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
