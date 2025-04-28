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



        //logica para editar


        [HttpGet]
        public IActionResult Editar(int id)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    return RedirectToAction("Login", "Login");
                }


                var routeProductos = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
                var content = System.IO.File.ReadAllText(routeProductos);
                var productos = JsonSerializer.Deserialize<List<Producto>>(content);

                Producto producto = productos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return NotFound();

            }
        }

        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
           
            string usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Login");
            }

            if (ModelState.IsValid)
            {
                var routeProductos = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");
                var content = System.IO.File.ReadAllText(routeProductos);
                var productos = JsonSerializer.Deserialize<List<Producto>>(content);

                var index = productos.FindIndex(p => p.Id == producto.Id);

                if (index != -1)
                {
                    productos[index] = producto;

                    string contenido = JsonSerializer.Serialize<List<Producto>>(productos, new JsonSerializerOptions { WriteIndented = true });

                    System.IO.File.WriteAllText(routeProductos, contenido);
                    

                    return RedirectToAction("Index", "Producto");
                }

                return NotFound();
            }
                return View(producto);
        }

    }
}
