using ExamDaniel.Models;
using Microsoft.AspNetCore.Mvc;
using ExamDaniel.bitacora; 

namespace ExamenAbril.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;

        public LoginController()
        {
            _usuarioServicio = new UsuarioServicio();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            var user = _usuarioServicio.ValidateUser(usrNombre, password);

            if (user != null)
            {
                HttpContext.Session.SetString("UsrNombre", user.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", user.NombreCompleto);

                // Registro en bitácora: login exitoso
                BitacoraManager.RegistrarEvento("Acceso", $"Inicio de sesión exitoso: {user.UsrNombre}");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Registro en bitácora: intento fallido
                BitacoraManager.RegistrarEvento("Error", $"Intento fallido de inicio de sesión con el usuario: {usrNombre}");

                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            var usuario = HttpContext.Session.GetString("UsrNombre");

            // Registro en bitácora: cierre de sesión
            if (!string.IsNullOrEmpty(usuario))
            {
                BitacoraManager.RegistrarEvento("Acceso", $"Cierre de sesión: {usuario}");
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
