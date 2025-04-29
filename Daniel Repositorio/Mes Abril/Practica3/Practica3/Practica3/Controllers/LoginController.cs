using Microsoft.AspNetCore.Mvc;
using Practica3.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging; 

namespace Practica3.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly ILogger<LoginController> _logger;  // Agregamos el logger para este controller

        // Inyección de dependencias para el servicio de usuario y el logger
        public LoginController(UsuarioServicio usuarioServicio, ILogger<LoginController> logger)
        {
            _usuarioServicio = usuarioServicio;
            _logger = logger;  // Inyectamos el logger
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            var users = _usuarioServicio.ValidateUser(usrNombre, password);

            if (users != null)
            {
                // Log para inicio de sesión exitoso
                _logger.LogInformation("Inicio de sesión exitoso para el usuario: {UsrNombre}", usrNombre);

                // Guardar en la sesión
                HttpContext.Session.SetString("UsrNombre", users.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", users.NombreCompleto);

                // Generar el token (puedes usar JWT o algo más simple, por ejemplo una GUID)
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }

            // Log para intento fallido de inicio de sesión
            _logger.LogWarning("Intento fallido de inicio de sesión para el usuario: {UsrNombre}", usrNombre);

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            // Log para cuando el usuario cierra sesión
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            _logger.LogInformation("El usuario {UsrNombre} cerró sesión.", usrNombre);

            // ELIMINA las variables de sesión
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
