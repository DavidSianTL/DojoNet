using System.Text.Json;
using CRUD_Evaluacion_Mensual_Abril.Models;
using Microsoft.AspNetCore.Mvc;


namespace CRUD_Evaluacion_Mensual_Abril.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly BitacoraService _bitacora;

        public LoginController(UsuarioServicio usuarioServicio, BitacoraService bitacora)
        {
            _usuarioServicio = usuarioServicio;
            _bitacora = bitacora;
        }

        public IActionResult Login()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            var user = _usuarioServicio.ValidateUser(usrNombre, password);
            if (user != null)
            {
                // Guardar en la sesión
                HttpContext.Session.SetString("UsrNombre", user.UsrNombre);
                HttpContext.Session.SetString("NombreCompleto", user.NombreCompleto);

                // Generar el token
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("Token", token);

                // Registrar inicio de sesión exitoso
                _bitacora.RegistrarEvento(HttpContext, usrNombre, "Inicio de sesión exitoso");
                var tokenSesion = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("SesionToken", tokenSesion);

                GuardarTokenSesion(usrNombre, tokenSesion); // guarda el token en el archivo

                return RedirectToAction("Index", "Home");
            }

            // Registrar intento fallido
            _bitacora.RegistrarEvento(HttpContext, usrNombre, "Intento fallido de inicio de sesión");

            // Mostrar mensaje de error
            TempData["Error"] = "Usuario o contraseña incorrectos";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");

            // Registrar logout
            if (!string.IsNullOrEmpty(usrNombre))
            {
                _bitacora.RegistrarEvento(HttpContext, usrNombre, "Cierre de sesión");
            }
            EliminarTokenSesion(HttpContext.Session.GetString("UsrNombre"));
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        private void EliminarTokenSesion(string usr)
        {
            string archivo = "sesiones.json";
            if (!System.IO.File.Exists(archivo)) return;

            var sesiones = JsonSerializer.Deserialize<Dictionary<string, string>>(System.IO.File.ReadAllText(archivo)) ?? new();

            if (sesiones.ContainsKey(usr))
            {
                sesiones.Remove(usr);
                string json = JsonSerializer.Serialize(sesiones, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(archivo, json);
            }
        }

        private void GuardarTokenSesion(string usr, string token)
        {
            string archivo = "sesiones.json";
            Dictionary<string, string> sesiones;

            if (System.IO.File.Exists(archivo))
            {
                string contenido = System.IO.File.ReadAllText(archivo);
                sesiones = JsonSerializer.Deserialize<Dictionary<string, string>>(contenido) ?? new();
            }
            else
            {
                sesiones = new Dictionary<string, string>();
            }

            sesiones[usr] = token;
            string json = JsonSerializer.Serialize(sesiones, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(archivo, json);
        }
    }
}