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
            return nombreCompleto != null ? $"[Usuario: {nombreCompleto}]" : "[Usuario: No identificado]";
        }

        // Función para registrar en log.txt
        private void RegistrarLog(string accion, string descripcion)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {NombreCompletoLog()} [Acción: {accion}] {descripcion}{Environment.NewLine}";
            System.IO.File.AppendAllText("log.txt", logEntry);
        }

        public IActionResult Login()
        {
            return View();
        }

        // Acción que maneja el inicio de sesión
        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            try
            {
                var validarUsuario = _usuario.ValidateUser(usrNombre, password);

                HttpContext.Session.SetString("UsrNombre", validarUsuario.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", validarUsuario.NombreCompleto);
                HttpContext.Session.SetString("MostrarAlerta", "true");

                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                RegistrarLog("Login", $"Inicio de sesión exitoso para el usuario '{usrNombre}'.");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                RegistrarLog("Login", $"Error al iniciar sesión para el usuario '{usrNombre}'. Detalle: {ex.Message}");
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Acción que maneja el registro de un nuevo usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserViewModel usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RegistrarLog("Registro", $"Error de validación al registrar usuario '{usuario.UsrNombre}'.");
                    return View(usuario);
                }

                var usersService = new UsersService();

                var usuarioExiste = usersService.ObtenerUsuarioPorNombre(usuario.UsrNombre);

                if (usuarioExiste != null)
                {
                    RegistrarLog("Registro", $"Intento de registrar usuario duplicado: '{usuario.UsrNombre}'.");
                    ModelState.AddModelError("", "El nombre de usuario ya existe, por favor elija otro.");
                    return View(usuario);
                }

                usersService.GuardarUsuario(usuario);

                HttpContext.Session.SetString("UsrNombre", usuario.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", usuario.NombreCompleto);
                HttpContext.Session.SetString("MostrarAlerta", "true");

                RegistrarLog("Registro", $"Registro exitoso del usuario '{usuario.UsrNombre}'.");

                return RedirectToAction("Login", "Login");
            }
            catch (Exception e)
            {
                RegistrarLog("Registro", $"Error inesperado al registrar usuario '{usuario.UsrNombre}'. Detalle: {e.Message}");
                ModelState.AddModelError("", "Ocurrió un error inesperado. Intente nuevamente.");
                return View(usuario);
            }
        }

        // Función para cerrar sesión
        public IActionResult Logout()
        {
            RegistrarLog("Logout", "El usuario cerró sesión.");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
