using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Proyecto.Filters;
using Proyecto.Utils;
using Microsoft.AspNetCore.Authorization; 

namespace Proyecto.Controllers
{
    [RequireLogin]
    public class AuthController : Controller
    {
        private const string SessionUserId = "UsuarioId";

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Login(string usuarioId, string contraseña)
        {
            try
            {
                var usuarios = JsonDb.ObtenerUsuarios();
                var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId && u.Contraseña == contraseña);

                if (usuario != null)
                {
                    HttpContext.Session.SetString(SessionUserId, usuario.UsuarioId);
                    Logger.RegistrarAccion(usuario.UsuarioId, "Inició sesión");
                    TempData["Bienvenida"] = $"Bienvenido, {usuario.UsuarioId}";
                    return RedirectToAction("Index", "Dashboard");
                }

                TempData["Error"] = "Credenciales incorrectas.";
                Logger.RegistrarAccion(usuarioId, "[LOGIN FALLIDO] Intento de inicio de sesión con credenciales incorrectas");
                return View();
            }
            catch (Exception ex)
            {
                Logger.RegistrarError($"Login: {usuarioId}", ex);
                TempData["Error"] = "Error interno al iniciar sesión.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            var usuarioId = HttpContext.Session.GetString(SessionUserId);

            if (!string.IsNullOrEmpty(usuarioId))
                Logger.RegistrarAccion(usuarioId, "Cerró sesión");

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
