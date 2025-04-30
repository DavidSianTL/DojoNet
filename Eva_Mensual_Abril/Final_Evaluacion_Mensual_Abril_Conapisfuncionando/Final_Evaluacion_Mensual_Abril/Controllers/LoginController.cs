using Microsoft.AspNetCore.Mvc;
using Final_Evaluacion_Mensual_Abril.Models;
using Microsoft.AspNetCore.Http;
using Final_Evaluacion_Mensual_Abril.Services;
using Microsoft.AspNetCore.Authorization;
using Proyecto1.Services;


namespace Final_Evaluacion_Mensual_Abril.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly BitacoraService _bitacora;

        public LoginController(UsuarioServicio usuarioServicio, BitacoraService bitacora)
        {
            _usuarioServicio = usuarioServicio;
            _bitacora = bitacora;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            var users = _usuarioServicio.ValidateUser(usrNombre, password);
            if (users != null)
            {
                HttpContext.Session.SetString("UsrNombre", users.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", users.NombreCompleto);
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                _bitacora.RegistrarEvento(new LogEntry
                {
                    Usuario = users.UsrNombre,
                    Accion = "Login",
                    Controlador = "Login",
                    Descripcion = "Inicio de sesión exitoso"
                });

                return RedirectToAction("Index", "Home");
            }

            _bitacora.RegistrarEvento(new LogEntry
            {
                Usuario = usrNombre,
                Accion = "Login",
                Controlador = "Login",
                Descripcion = "Intento fallido de inicio de sesión",
                EsError = true
            });

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            var usuario = HttpContext.Session.GetString("UsrNombre");

            if (!string.IsNullOrEmpty(usuario))
            {
                _bitacora.RegistrarEvento(new LogEntry
                {
                    Usuario = usuario,
                    Accion = "Logout",
                    Controlador = "Login",
                    Descripcion = "Cierre de sesión"
                });
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}