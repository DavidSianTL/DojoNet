using Microsoft.AspNetCore.Mvc;
using Reto.Models.Usuario;
using System.Globalization;
using System.Text.Json;

namespace Reto.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                //leer el registro de usuarios (archivo JSON)
                string rute = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Usuarios.json");

                string contenido = System.IO.File.ReadAllText(rute);

                List<Usuario> ListaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido);


                //Buscamos un usuario que coincida con el ingresado
                bool valido = ListaUsuarios.Any(u =>
                    u.NombreUsuario == usuario.NombreUsuario &&
                    u.Password == usuario.Password);


                //si la variable valido es true redirigimos al metodo que muestra la vista de Productos
                //                      que agrega productos nuevos
                if (valido)
                {
                    return RedirectToAction("Producto");
                }

                // si no se muestra un mensaje de error de contraseña (retroalimentación)
                ViewBag.Mensaje = "Usuario o contraseña incorrctos";

            }

            //volvemos a mostrar la vista que tiene el nombre del metodo (la vista login,>>form<<)
            return View();
        }

        [HttpGet]
        public IActionResult Producto()
        {
            return View("/Views/Producto/Producto.cshtml");
        }
    }
}