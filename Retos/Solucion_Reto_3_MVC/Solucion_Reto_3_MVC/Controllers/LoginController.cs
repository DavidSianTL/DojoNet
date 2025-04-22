using Microsoft.AspNetCore.Mvc;
using Solucion_Reto_3_MVC.Models;

namespace Solucion_Reto_3_MVC.Controllers
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

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                var user = _usuarioServicio.ObtenerUsuarios(username, password);
                if (user != null)
                {
                    // Guardar el usuario en la sesión
                    HttpContext.Session.SetString("UserName", user.UserName);

                    var token = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("Token", token);

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
                return View();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
