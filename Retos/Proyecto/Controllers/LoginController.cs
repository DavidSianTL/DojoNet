using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using Proyecto.Filters;

namespace Proyecto.Controllers
{
    [RequireLogin]
    public class AuthController : Controller
    {
        private const string SessionUserId = "UsuarioId";

        private void EscribirLog(string mensaje)
        {
            try
            {
                string logDir = Path.Combine(AppContext.BaseDirectory, "App_Data");
                string logPath = Path.Combine(logDir, "log.txt");

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensaje}{Environment.NewLine}";
                System.IO.File.AppendAllText(logPath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] al escribir log: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuarioId, string contraseña)
        {
            try
            {
                var usuarios = JsonDb.ObtenerUsuarios();
                var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId && u.Contraseña == contraseña);

                if (usuario != null)
                {
                    HttpContext.Session.SetString(SessionUserId, usuario.UsuarioId);
                    EscribirLog($"Usuario {usuario.UsuarioId} inició sesión.");
                    TempData["Bienvenida"] = $"Bienvenido, {usuario.UsuarioId}";
                    return RedirectToAction("Index", "Dashboard");
                }

                TempData["Error"] = "Credenciales incorrectas.";
                EscribirLog($"[LOGIN FALLIDO] Usuario: {usuarioId}");
                return View();
            }
            catch (Exception ex)
            {
                EscribirLog($"[ERROR] Login: {usuarioId} - {ex.Message}, StackTrace: {ex.StackTrace}");
                TempData["Error"] = "Error interno al iniciar sesión.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            var usuarioId = HttpContext.Session.GetString(SessionUserId);

            if (!string.IsNullOrEmpty(usuarioId))
                EscribirLog($"Usuario {usuarioId} cerró sesión.");

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
