using Microsoft.AspNetCore.Mvc;
using RetoTres.Models;
using System.Text.Json;


namespace RetoTress.Controllers
{
    public class LoginController : Controller
    {
        // GET: muestra el formulario de login vacío
        public IActionResult Login()
        {
            return View();
        }

        // POST: procesa el formulario
        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Leer el archivo JSON
                string ruta = Path.Combine(Directory.GetCurrentDirectory(), "Data", "usuarios.json");

                string contenido = System.IO.File.ReadAllText(ruta);

                List<Usuario> lista = JsonSerializer.Deserialize<List<Usuario>>(contenido);

                // Buscar coincidencia
                bool valido = lista.Any(u =>
                    u.NombreUsuario == usuario.NombreUsuario &&
                    u.Password == usuario.Password);

                if (valido)
                {
                    return RedirectToAction("Bienvenido");
                }

                ViewBag.Mensaje = "Usuario o contraseña incorrectos";
            }

            return View();
        }

        public IActionResult Producto()
        {
            return View();
        }
    }
}
