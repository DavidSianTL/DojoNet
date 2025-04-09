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

                    return RedirectToAction("Index", "Home");
                }

                // si no es valido se muestra un mensaje de error de contraseña (retroalimentación)
                ViewBag.Mensaje = "Usuario o contraseña incorrctos";

            }

            //volvemos a mostrar la vista que tiene el nombre del metodo (la vista login,>>form<<)
            return View();
        }


        //Logout
        public IActionResult Logout()
        {
            //Eliminar la sisión
            HttpContext.Session.Clear();
            //Redirigir al login
            return RedirectToAction("Login","Login");
        }
        
    }
}