using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class UsuarioController : Controller
    {
        // Instanciamos el DAO
        private readonly daoUsuarioWSAsync _daoUsuarioWS;

        // Constructor para inicializar la cadena de conexión
        public UsuarioController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoUsuarioWS = new daoUsuarioWSAsync(_connectionString);
        }

        // Acción que muestra la vista de usuarios
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Acción para agregar un nuevo usuario
        // Solo SuperAdmin y Admin pueden ver la lista de usuarios
        [AuthorizeRole("SuperAdmin", "Admin")]
        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return View("Agregar", "Usuario");
        }

        // Acción para editar un usuario existente
        // Solo SuperAdmin puede editar usuarios
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public IActionResult EditarUsuario()
        {
            return View("Editar", "Usuario"); 
        }


        // Acción para "eliminar" un usuario (en realidad, cambiar su estado a inactivo)
        [HttpPost]
        public IActionResult EliminarUsuario(int Id)
        {
            // Aquí se llamaría al método para eliminar el usuario
            return RedirectToAction("Index");
        }




    }
}
