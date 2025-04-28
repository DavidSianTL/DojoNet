using CRUD.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            var routeProducts = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
            var content = System.IO.File.ReadAllText(routeProducts);
            var listaProductos = JsonSerializer.Deserialize<List<Producto>>(content);


            return View(listaProductos);
        }
    }
}
