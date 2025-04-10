using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Reto.Models;



namespace Reto.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public IActionResult Producto()
        {
            //si el usuario es valido estará guardado en la sesion
            string usuario = HttpContext.Session.GetString("Usuario");

            //si es null o vacío entonces no lo hemos guardado; si no lo hemos guardado entonces no era valido/ o no inició sesión
            if (string.IsNullOrEmpty(usuario)) return RedirectToAction("Login", "Login"); // y lo redirigimos a la vista login

            //si está en la sesión lo redirigimos a la vista Producto


            //Obtenemos la ruta del archivo json que guarda los productos

            string route = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");

            //leer el contenido del json y guardarlo en una variable
            String contenido = System.IO.File.ReadAllText(route);

            //Deserealizar el contenido del json y convertirlo a una lista de objetos tipo objeto
            List<Producto> ListaProductos = JsonSerializer.Deserialize<List<Producto>>(contenido);
            

            return View(ListaProductos);
        }
    }
}
