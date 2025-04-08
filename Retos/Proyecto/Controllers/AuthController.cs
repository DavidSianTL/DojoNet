using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Proyecto.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuarioId, string contraseña)
        {
            var usuarios = JsonDb.ObtenerUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId && u.Contraseña == contraseña);

            if (usuario != null)
            {
                HttpContext.Session.SetString("usuario", usuario.UsuarioId);
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
    }
}
