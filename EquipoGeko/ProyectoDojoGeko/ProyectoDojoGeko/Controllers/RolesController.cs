using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession] 
    [AuthorizeRole("SuperAdmin")] // Solo SuperAdmin puede administrar roles
    public class RolesController : Controller
    {
        private readonly daoRolesWSAsync _dao;

        // Constructor que inicializa la conexión con la base de datos
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

        // Confirmar cambiar estado del rol a inactivo.
        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            // Cambiar el estado del rol a inactivo en lugar de eliminarlo físicamente
            await _dao.DesactivarRolAsync(id); 
            return RedirectToAction(nameof(Index));
        }
    }
}
