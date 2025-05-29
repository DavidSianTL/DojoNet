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
            catch (Exception e)
            {
                // En caso de error, mostramos un mensaje de error
                ViewBag.Mensaje = "Error al procesar la solicitud: " + e.Message;
                // Retornamos la vista de inicio de sesión con el mensaje de error
                return View();
            }
        }

        // NUEVAS ACCIONES PARA CAMBIO DE CONTRASEÑA

        // Acción GET para mostrar la vista de cambio de contraseña
        [HttpGet]
        public IActionResult CambioContraseña()
        {
            try
            {
                // Verificar que el usuario esté autenticado (opcional para presentación)
                var usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    // Para presentación, permitir acceso sin sesión
                    ViewBag.Usuario = "Usuario Demo";
                }
                else
                {
                    ViewBag.Usuario = usuario;
                }

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = "Error al cargar la página: " + e.Message;
                return View();
            }
        }

        // Acción POST para procesar el cambio de contraseña
        [HttpPost]
        public IActionResult CambioContraseña(string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            try
            {
                // Obtener usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    // Para presentación, usar usuario demo
                    ViewBag.Usuario = "Usuario Demo";
                }
                else
                {
                    ViewBag.Usuario = usuario;
                }

                // Validaciones básicas
                if (string.IsNullOrEmpty(contraseñaActual) || string.IsNullOrEmpty(nuevaContraseña) || string.IsNullOrEmpty(confirmarContraseña))
                {
                    ViewBag.Mensaje = "Todos los campos son obligatorios.";
                    return View();
                }

                if (nuevaContraseña != confirmarContraseña)
                {
                    ViewBag.Mensaje = "Las contraseñas no coinciden.";
                    return View();
                }

                if (nuevaContraseña.Length < 8)
                {
                    ViewBag.Mensaje = "La nueva contraseña debe tener al menos 8 caracteres.";
                    return View();
                }

                // Para presentación: simular validación exitosa
                if (!string.IsNullOrEmpty(usuario) && usuario != "Usuario Demo")
                {
                    // Verificar la contraseña actual usando el método existente
                    var usuarioValido = _daoTokenUsuario.ValidarUsuario(usuario, contraseñaActual);

                    if (usuarioValido == null)
                    {
                        ViewBag.Mensaje = "La contraseña actual es incorrecta.";
                        return View();
                    }
                }

                // Simular cambio exitoso
                ViewBag.Mensaje = "¡Contraseña cambiada exitosamente! (Modo presentación)";

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = "Error al procesar la solicitud: " + e.Message;
                ViewBag.Usuario = HttpContext.Session.GetString("Usuario") ?? "Usuario Demo";
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