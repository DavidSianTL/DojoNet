using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
    public class PermisosController : Controller
    {
        // Instancia del DAO para acceder a la base de datos
        private readonly daoPermisosWSAsync _dao;

        // Constructor con inyección de la cadena de conexión
        public PermisosController()
        {
            //string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Conexion manual para pruebas rapidas...
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoPermisosWSAsync(connectionString);
        }

        // Acción que muestra la lista de permisos
        public async Task<IActionResult> Index()
        {
            var permisos = await _dao.ObtenerPermisosAsync();
            return View(permisos);
        }

        //CAMBIAR NOMBRE A LA VISTA DE "LISTAR" POR EL NOMBRE VERDADERO.
        //// Esta acción recibe un ID de permiso y muestra los detalles del mismo.
        public async Task<IActionResult> LISTAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // Acción que procesa el formulario de creación (POST)
        //CAMBIAR NOMBRE A LA VISTA DE "CREAR" POR EL NOMBRE VERDADERO.
        public IActionResult CREAR()
        {
            return View();
        }

        // Acción que procesa el formulario de creación (POST)
        // CAMBIAR NOMBRE A LA VISTA DE "CREAR" POR EL NOMBRE VERDADERO.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CREAR(PermisoViewModel permiso)
        {
            if (ModelState.IsValid)
            {
                await _dao.InsertarPermisoAsync(permiso);
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // Esta acción recibe un ID de permiso y muestra el formulario de edición.
        //CAMBIAR NOMBRE A LA VISTA DE "EDITAR" POR EL NOMBRE VERDADERO.
        public async Task<IActionResult> EDITAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }


        // Acción que EDITA permiso (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EDITAR(PermisoViewModel permiso)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarPermisoAsync(permiso);
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // Acción que muestra el formulario de confirmación para eliminar
        //CAMBIAR NOMBRE A LA VISTA DE "ELIMINAR" POR EL NOMBRE VERDADERO.
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return View(permiso);
        }

        // Acción que elimina al empleado (POST)
        [HttpPost, ActionName("ELIMIINAR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarPermisoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
