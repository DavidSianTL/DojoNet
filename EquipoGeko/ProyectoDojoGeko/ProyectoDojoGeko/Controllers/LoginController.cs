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
        public IActionResult Index() // Se cambió el nombre de Login() a Index() para que coincida con la vista Index.cshtml
        {
            return View(); // Muestra la vista Views/Login/Index.cshtml
        }

        // Acción que maneja el inicio de sesión
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password) // Se cambió también aquí para que coincida con la acción GET
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
                    return View(); // Se cambió RedirectToAction por View() para mantener el mensaje de error
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Se cambió RedirectToAction por View() para mostrar el error en la misma vista
            }
        }

        // Método para cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); // Se ajustó el nombre de la acción de destino a "Index"
        }
    }
}
