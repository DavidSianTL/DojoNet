using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TiendaEnLinea_MR.Models;
using Microsoft.Extensions.Logging;

namespace TiendaEnLinea_MR.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicioModel _usuarioServicio;
        private readonly ILogger<LoginController> _logger;
        public LoginController(UsuarioServicioModel usuarioServicio , ILogger<LoginController> logger)
        {
            _usuarioServicio = usuarioServicio;
            _logger = logger;
        }

        public IActionResult Login()
        {
            _logger.LogInformation("Accediendo a la vista de Login");
            return View();
        }


        // POST: Login
        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            _logger.LogInformation("Intentando iniciar sesión para el correo: {Correo}", correo);  

            var user = _usuarioServicio.ValidateUser(correo, password);
            if (user != null)
            {
                // Guardar en la sesión
                HttpContext.Session.SetString("Correo", user.Correo);
                HttpContext.Session.SetString("NombreCompleto", user.NombreCompleto);

                // Generar el token
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                _logger.LogInformation("Inicio de sesión exitoso para el correo: {Correo}", correo);  
                return RedirectToAction("Index", "Home");
            }

            // Si la validación falla
            _logger.LogWarning("Falló el inicio de sesión para el correo: {Correo}", correo);  
            ViewBag.ErrorMessage = "Correo o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            _logger.LogInformation("Usuario {Correo} ha cerrado sesión", HttpContext.Session.GetString("Correo"));  
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }

    }
}

