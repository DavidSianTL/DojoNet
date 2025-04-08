using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TiendaEnLinea_MR.Models;


namespace wAppGestionVacacional.Controllers
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
            return View("~/Views/Home/Login.cshtml");
        }


        // POST: Login
        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            var user = _usuarioServicio.ValidateUser(correo, password);
            if (user != null)
            {
                // Guardar en la sesión
                HttpContext.Session.SetString("Correo", user.Correo);
                HttpContext.Session.SetString("NombreCompleto", user.NombreCompleto);

                // Generar el token (puedes usar JWT o algo más simple, por ejemplo una GUID)
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Correo o contraseña incorrectos";
            return View("~/Views/Home/Index.cshtml");
        }

    }
}

