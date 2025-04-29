using ExamDaniel.Models;
using Microsoft.AspNetCore.Mvc;

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

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
