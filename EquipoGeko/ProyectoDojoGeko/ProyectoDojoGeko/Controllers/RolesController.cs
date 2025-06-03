using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    public class RolesController : Controller
    {
        // Instancia del DAO para acceder a la base de datos
        private readonly daoRolesWSAsync _dao;

        // Constructor que inyecta la configuración para obtener la cadena de conexión
        public RolesController(IConfiguration configuration)
        {
            //string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Cadena de conexión manual para pruebas rápidas...
            string connectionString = "Server=DESKTOP-LPDU6QD\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoRolesWSAsync(connectionString);
        }

        // Acción que muestra la lista de roles
        public async Task<IActionResult> Index()
        {
            var roles = await _dao.ObtenerRolesAsync();
            return View(roles);
        }

        // Acción que muestra el formulario de creación de rol
        public IActionResult Crear()
        {
            return View();
        }

        // Acción que procesa el formulario de creación (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CREAR(RolesViewModel rol)
        {
            if (ModelState.IsValid)
            {
                await _dao.InsertarRolAsync(rol);
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // Acción que muestra los detalles de un rol por ID
        public async Task<IActionResult> LISTAR(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Acción que muestra el formulario de edición de rol
        public async Task<IActionResult> EDITAR(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Acción que procesa la edición (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EDITAR(RolesViewModel rol)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarRolAsync(rol);
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // Acción que muestra el formulario de confirmación para eliminar
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Acción que elimina el rol (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarRolAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

