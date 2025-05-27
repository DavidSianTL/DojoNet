using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    public class LoginController : Controller
    {
        // Instanciamos el DAO
        private readonly daoUsuarioWSAsync _daoUsuarioWS;

        // Constructor para inicializar la cadena de conexión
        public LoginController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoUsuarioWS = new daoUsuarioWSAsync(_connectionString);
        }

        // Acción que muestra la vista de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            return View("Index", "Login");
        }

        // Acción que maneja el inicio de sesión
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                // Validar usuario en la base de datos
                var usuario = await _daoUsuarioWS.ValidateUser(username, password);

                if (usuario != null)
                {
                    // Usuario válido - crear sesión
                    HttpContext.Session.SetString("UserName", usuario.Username);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Usuario no válido
                    ViewBag.Error = "Usuario o contraseña incorrectos.";
                    return RedirectToAction("Login", "Login");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Login", "Login");
            }
        }

        // Método para cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }


    }
}
