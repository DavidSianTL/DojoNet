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
                //obtener la ruta del registro de usuarios (archivo JSON) y la guardamos en una variable
                string route = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Usuarios.json");

                //leemos el contenido del json y lo guardamos en una variable
                string contenido = System.IO.File.ReadAllText(route);

                //convertimos el contenido del json en una lista de objetos de la clase Usuario
                List<Usuario> ListaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido);


                //Buscamos un usuario que coincida con el ingresado
                var UsuarioValido = ListaUsuarios.FirstOrDefault(u =>
                    u.NombreUsuario == usuario.NombreUsuario &&
                    u.Password == usuario.Password);


                //se asegura que el usuario sea valido
                if (UsuarioValido !=null)
                {
                    //se guarda el nombre del usuario en la sesión
                    HttpContext.Session.SetString("Usuario", UsuarioValido.NombreUsuario);

                    return RedirectToAction("Producto");
                }

                // si no es valido se muestra un mensaje de error de contraseña (retroalimentación)
                ViewBag.Mensaje = "Usuario o contraseña incorrctos";

            }

            //volvemos a mostrar la vista que tiene el nombre del metodo (la vista login,>>form<<)
            return View();
        }

        [HttpGet]
        public IActionResult Producto()
        {
            //si el usuario es valido estará guardado en la sesion
            string usuario = HttpContext.Session.GetString("Usuario");
            
//si es null o vacío entonces no lo hemos guardado; si no lo hemos guardado entonces no era valido/ o no inició sesión
            if (string.IsNullOrEmpty(usuario))   return RedirectToAction("Login"); // y lo redirigimos a la vista login
            
            //si está en la sesión lo redirigimos a la vista Producto
            return View("/Views/Producto/Producto.cshtml");
        }
    }
}