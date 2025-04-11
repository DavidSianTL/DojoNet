using Microsoft.AspNetCore.Mvc;
using Proyercto1.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Proyercto1.Controllers
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
            var users = _usuarioServicio.ValidateUser(usrNombre, password);
            if (users != null)
            {
                // Guardar en la sesión
                HttpContext.Session.SetString("UsrNombre", users.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", users.NombreCompleto);

                // Generar el token 
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }

            // Si la validación falla, logueamos el error
            Logger.LogError(usrNombre, "Usuario o contraseña incorrectos");

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            // ELIMINA las variables de session
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}

