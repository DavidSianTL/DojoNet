using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;
using Microsoft.AspNetCore.Http;
using _Evaluacion_Mensual_Abril.Services;
using System;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginServicio _loginServicio;
        private readonly LoggerServices _loggerServices;

        public LoginController(LoginServicio loginServicio)
        {
            _loginServicio = loginServicio;
            _loggerServices = new LoggerServices(); 
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var login = _loginServicio.ValidateUser(Username, Password);
            string fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (login != null)
            {
                HttpContext.Session.SetString("NombreCompleto", login.NombreCompleto);

                ViewBag.SuccessMessage = $"Bienvenido {login.NombreCompleto}";
                _loggerServices.RegistrarAccion(Username, $"Login Exitoso - {fechaHora}");

                return View();
            }

            ViewBag.ErrorMessage = "Email o Contraseña incorrectos";
            _loggerServices.RegistrarAccion(Username, $"Login Fallido - {fechaHora}");

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
