using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserViewModel _usuario;

        public LoginController(UserViewModel usuario)
        {
            _usuario = usuario;
        }

        // Función global para obtener el nombre completo del usuario
        public string NombreCompletoLog()
        {
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            if (nombreCompleto != null)
            {
                return "Usuario: " + nombreCompleto;
            }
            else
            {
                return "No se pudo acceder al nombre completo del usuario.";
            }
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

                // Sí pasa la validación, se guarda en sesión
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
            var message = "Saliendo de la sesión";
            System.IO.File.AppendAllText("log.txt", DateTime.Now + NombreCompletoLog() + message + Environment.NewLine);
            return RedirectToAction("Login", "Login");

        }

    }
}
