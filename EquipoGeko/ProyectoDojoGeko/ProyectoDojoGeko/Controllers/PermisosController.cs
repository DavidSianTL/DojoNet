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

        // Constructor que inicializa la conexión con la base de datos
        public PermisosController()
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Conexión directa para pruebas rápidas
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoPermisosWSAsync(connectionString);
        }

        // Acción que muestra la lista de permisos
        public async Task<IActionResult> Index()
        {
            var permisos = await _dao.ObtenerPermisosAsync();
            return View(permisos);
        }

        // Acción que muestra los detalles de un permiso específico
        public async Task<IActionResult> LISTAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Acción que muestra el formulario para crear un nuevo permiso
        public IActionResult CREAR()
        {
            return View();
        }

        // Acción que procesa el formulario de creación (POST)
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

        // Acción que muestra el formulario de edición para un permiso existente
        public async Task<IActionResult> EDITAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Acción que procesa la edición de un permiso (POST)
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

        // Acción que muestra la confirmación para eliminar un permiso
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Acción que elimina el permiso (POST)
        [HttpPost, ActionName("ELIMINAR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarPermisoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
