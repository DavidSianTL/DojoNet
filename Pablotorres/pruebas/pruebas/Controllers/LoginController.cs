using Microsoft.AspNetCore.Mvc;
using pruebas.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using pruebas.Utils; 

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

        [HttpPost]
        public IActionResult Login(string Username, string password)
        {
            var users = _usuarioServicio.ValidateUser(Username, password);
            if (users != null)
            {
                HttpContext.Session.SetString("Username", users.Username);
                HttpContext.Session.SetString("NombreCompleto", users.NombreCompleto);

                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }

    
            Logger.LogError(Username, "Todo mal");

            ViewBag.ErrorMessage = "Todo Mal, Ingrese de nuevo";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
