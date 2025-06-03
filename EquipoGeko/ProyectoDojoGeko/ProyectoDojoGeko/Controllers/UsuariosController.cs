using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class UsuariosController : Controller
    {
        // Instanciamos el DAO
        private readonly daoUsuarioWSAsync _daoUsuarioWS;

        // Instanciamos el DAO de bitácora
        private readonly daoBitacoraWSAsync _daoBitacora;

        // Instanciamos el DAO de log
        private readonly daoLogWSAsync _daoLog;

        // Constructor para inicializar la cadena de conexión
        public UsuariosController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=DESKTOP-LPDU6QD\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoUsuarioWS = new daoUsuarioWSAsync(_connectionString);
            // Inicializamos el DAO de bitácora con la misma cadena de conexión
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            // Inicializamos el DAO de log con la misma cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Acción que muestra la vista de usuarios
        [AuthorizeRole("SuperAdmin", "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtenemos la lista de usuarios desde el DAO
                var usuarios = await _daoUsuarioWS.ObtenerUsuariosAsync();

                // Si la lista de usuarios no es nula y tiene elementos, la devolvemos a la vista
                if (usuarios != null && usuarios.Any())
                    return View(usuarios);


                // Guardamos el token y el nombre de usuario en la sesión
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema");

                // Insertamos en la bítacora el inicio de sesión exitoso
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Login",
                    Descripcion = $"Inicio de sesión exitoso para el usuario {usuario}.",
                    FK_IdUsuario = (int)idUsuario,
                    FK_IdSistema = (int)idSistema
                });

                // Redirigimos y le pasamos la lista de usuarios a la vista
                return View(new List<UsuarioViewModel>());

            }
            catch (Exception e)
            {

                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");

                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Login",
                    Descripcion = $"Error en el proceso de login para usuario {usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View(new List<UsuarioViewModel>());
            }

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
