using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using wAppGestionVacacional.Models;


namespace wAppGestionVacacional.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;


        public LoginController(UsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;

        }

        public IActionResult Login()
        {
            return View();
        }
       

        // POST: Login
        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            var user = _usuarioServicio.ValidateUser(usrNombre, password);
            if (user != null)
            {
                // Guardar en la sesión
                HttpContext.Session.SetString("UsrNombre", user.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", user.NombreCompleto);

                // Generar el token (puedes usar JWT o algo más simple, por ejemplo una GUID)
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View();
        }


        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Login");
        }
    }
}
