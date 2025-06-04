using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class PermisosController : Controller
    {
        private readonly daoPermisosWSAsync _dao;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public PermisosController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _dao = new daoPermisosWSAsync(connectionString);
            _daoLog = new daoLogWSAsync(connectionString);
            _daoBitacoraWS = new daoBitacoraWSAsync(connectionString);
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var permisos = await _dao.ObtenerPermisosAsync();
                return View(permisos);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(PermisoViewModel permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.InsertarPermisoAsync(permiso);
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                return View(permiso);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                return View(permiso);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PermisoViewModel permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.ActualizarPermisoAsync(permiso);
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                return View(permiso);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarPermisoAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
