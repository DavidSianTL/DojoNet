using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Examen_mes_abril.Models;

namespace Examen_mes_abril.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicioModel _usuarioServicio;

        public LoginController(UsuarioServicioModel usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            var user = _usuarioServicio.ValidateUser(correo, password);
            if (user != null)
            {
                // Guardar en la sesión
                HttpContext.Session.SetString("Correo", user.Correo);
                // Generar el token
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Correo o contraseña incorrectos";
            return View();

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutSuccess"] = true;

            return RedirectToAction("Login", "Login");
        }

    }
}

