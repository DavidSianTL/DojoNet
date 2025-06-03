using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
    
{
    //Requiere de la sesión de usuario autorizada
    [AuthorizeSession] 
    public class PermisosController : Controller
    {
        private readonly daoPermisosWSAsync _dao;

        // Constructor que inicializa la conexión con la base de datos
        public PermisosController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoPermisosWSAsync(connectionString);
        }

        // Mostrar lista de permisos
        public async Task<IActionResult> Index()
        {
            var permisos = await _dao.ObtenerPermisosAsync();
            return View(permisos);
        }

        // Ver detalles de un permiso
        [AuthorizeRole("SuperAdmin")]
        public async Task<IActionResult> LISTAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Formulario de creación de permiso
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public IActionResult CREAR()
        {
            return View();
        }

        // Crear permiso (POST)
        [AuthorizeRole("SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CREAR(PermisoViewModel permiso)
        {
            if (ModelState.IsValid)
            {
                await _dao.InsertarPermisoAsync(permiso);
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // Formulario para editar un permiso
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> EDITAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Editar permiso (POST)
        [AuthorizeRole("SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> EDITAR(PermisoViewModel permiso)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarPermisoAsync(permiso);
                return RedirectToAction(nameof(Index));
            }
            return View(permiso);
        }

        // Confirmación para eliminar permiso
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Eliminar permiso (POST)
        [AuthorizeRole("SuperAdmin")]
        [HttpPost, ActionName("ELIMINAR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarPermisoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
