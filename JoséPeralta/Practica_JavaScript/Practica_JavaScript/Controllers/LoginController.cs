using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practica_JavaScript.Models;

namespace Practica_JavaScript.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioViewModel _usuario;

        public LoginController(UsuarioViewModel usuario)
        {
            _usuario = usuario;
        }

        public IActionResult Login()
        {
            return View();
        }

        // Acción que maneja el inicio de sesión
        [HttpPost]
        /* public IActionResult Login(string usrNombre, string password)
         {
             // Validar el usuario
             var validarUsuario = _usuario.ValidateUser(usrNombre, password);
             if (validarUsuario != null)
             {
                 // Guardar en la sesión
                 HttpContext.Session.SetString("UsrNombre", validarUsuario.UsrNombre);
                 HttpContext.Session.SetString("NombreCompleto", validarUsuario.NombreCompleto);
                 HttpContext.Session.SetString("MostrarAlerta", "true"); // Activar la alerta

                 // Generamos el token 
                 var token = Guid.NewGuid().ToString();
                 HttpContext.Session.SetString("Token", token);

                 return RedirectToAction("Index", "Home");
             }
             else
             {
                 // Mostrar mensaje de error
                 ViewBag.Error = "Usuario o contraseña incorrectos";

                 return View();
             }
         }*/
        public IActionResult Login(string usrNombre, string password)
        {
            try
            {
                var validarUsuario = _usuario.ValidateUser(usrNombre, password);

                // Si pasa la validación, se guarda en sesión
                HttpContext.Session.SetString("UsrNombre", validarUsuario.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", validarUsuario.NombreCompleto);
                HttpContext.Session.SetString("MostrarAlerta", "true");

                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Error específico según validación
                ViewBag.Error = ex.Message;
                return View();
            }
        }



        // Función para volver a validar
        // En caso de que no se haya logrado validar el usuario
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");

        }

    }
}
