using CRUD.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CRUD.Controllers
{
    public class ProductoController : Controller
    {




        //Mostrar
        [HttpGet]
        public IActionResult Index()
        {

            var routeProducts = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
            var content = System.IO.File.ReadAllText(routeProducts);
            var listaProductos = JsonSerializer.Deserialize<List<Producto>>(content);


            return View(listaProductos);
        }

        //Crear
        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Crear(Producto producto)
        {
            var routeProductos = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
            var content = System.IO.File.ReadAllText(routeProductos);
            var productos = JsonSerializer.Deserialize<List<Producto>>(content);

           

            //generamos un id
            int idNuevo = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
            //añadimos ese id
            producto.Id = idNuevo;

            productos.Add(producto);
            string contenido = JsonSerializer.Serialize<List<Producto>>(productos, new JsonSerializerOptions { WriteIndented = true });

            System.IO.File.WriteAllText(routeProductos, contenido);


            return RedirectToAction("Index", "Producto");
        }

    }
}
