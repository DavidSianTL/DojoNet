using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession] // Requiere sesión activa
    [AuthorizeRole("SuperAdmin")] // Solo SuperAdmin puede administrar roles
    public class RolesController : Controller
    {
        private readonly daoRolesWSAsync _dao;

        // Constructor con conexión manual (igual que UsuarioController)
        public RolesController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoRolesWSAsync(connectionString);
        }

        // Mostrar lista de roles
        public async Task<IActionResult> Index()
        {
            var roles = await _dao.ObtenerRolesAsync();
            return View(roles);
        }

        // Mostrar formulario para crear rol
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // Guardar nuevo rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RolesViewModel rol)
        {
            if (ModelState.IsValid)
            {
                await _dao.InsertarRolAsync(rol);
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // Mostrar detalles de un rol
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Mostrar formulario para editar rol
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Guardar cambios del rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(RolesViewModel rol)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarRolAsync(rol);
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // Mostrar vista de confirmación para eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Eliminar (cambio de estado) del rol
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            // Cambiar el estado del rol a inactivo en lugar de eliminarlo físicamente
            await _dao.DesactivarRolAsync(id); // Este método lo vas a crear en el DAO
            return RedirectToAction(nameof(Index));
        }
    }
}
