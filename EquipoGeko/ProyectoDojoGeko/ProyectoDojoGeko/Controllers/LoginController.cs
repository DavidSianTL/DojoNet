using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Helper;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    public class LoginController : Controller
    {
        // Instanciamos el DAO de tokens
        private readonly daoTokenUsuario _daoTokenUsuario;

        // Constructor para inicializar la cadena de conexión
        public LoginController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos el DAO de tokens con la misma cadena de conexión
            _daoTokenUsuario = new daoTokenUsuario(_connectionString);

        }

        // Acción que muestra la vista de inicio de sesión
        [HttpGet]
        public IActionResult Index() // Se cambió el nombre de Login() a Index() para que coincida con la vista Index.cshtml
        {
            return View(); // Muestra la vista Views/Login/Index.cshtml
        }

        // Acción que maneja el inicio de sesión
        [HttpPost]
        public IActionResult Index(string usuario, string clave)
        {
            try
            {
                // Validamos el usuario y la clave usando el DAO de tokens
                var usuarioValido = _daoTokenUsuario.ValidarUsuario(usuario, clave);

                // Si el usuario es válido, generamos un token JWT y lo guardamos
                if (usuarioValido != null)
                {

                    // Verificamos si el usuario está activo
                    var jwtHelper = new JwtHelper();

                    // Generamos el token JWT para el usuario
                    var tokenModel = jwtHelper.GenerarToken(usuarioValido.IdUsuario, usuarioValido.Username);

                    // Guardamos el token en la base de datos
                    _daoTokenUsuario.GuardarToken(tokenModel);

                    // Guardamos el token y el nombre de usuario en la sesión
                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetString("Usuario", usuarioValido.Username);

                    // Redirigimos a la acción Index del controlador Home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Si el usuario no es válido, mostramos un mensaje de error
                    ViewBag.Mensaje = "Usuario o clave incorrectos.";
                    // Retornamos la vista de inicio de sesión con el mensaje de error
                    return View();
                }

            }
            catch(Exception e)
            {
                // En caso de error, mostramos un mensaje de error
                ViewBag.Mensaje = "Error al procesar la solicitud: " + e.Message;
                // Retornamos la vista de inicio de sesión con el mensaje de error
                return View();
            }

        }

        // Método para cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); // Se ajustó el nombre de la acción de destino a "Index"
        }

    }
}
