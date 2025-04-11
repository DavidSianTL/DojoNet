using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Reto.Models;



namespace Reto.Controllers
{
    public class ProductoController : Controller
    {
        private readonly string route = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");

        private List<Producto> LeerProductos()
        {
            if (!System.IO.File.Exists(route))
            {
                return new List<Producto>();
            }

            string contenido = System.IO.File.ReadAllText(route);

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return new List<Producto>();
            }

            List<Producto> ListaProductos = JsonSerializer.Deserialize<List<Producto>>(contenido) ??  new List<Producto>();

            return ListaProductos;
        }


        [HttpGet]
        public IActionResult Index()
        {
            //si el usuario es valido estará guardado en la sesion
            string usuario = HttpContext.Session.GetString("Usuario");

            //si es null o vacío entonces no lo hemos guardado; si no lo hemos guardado entonces no era valido/ o no inició sesión
            if (string.IsNullOrEmpty(usuario)) return RedirectToAction("Login", "Login"); // y lo redirigimos a la vista login
            List<Producto> Productos = LeerProductos();
            //si está en la sesión lo redirigimos a la vista Producto
            return View(Productos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        
    }
}
