using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class PermisosController : Controller
    {
        private readonly daoPermisosWSAsync _daoPermiso;// Acceso a datos de permisos
        private readonly daoLogWSAsync _daoLog;// Acceso a datos de logs
        private readonly daoBitacoraWSAsync _daoBitacora;// Acceso a datos de bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;// Acceso a datos de roles de usuario

        public PermisosController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoPermiso = new daoPermisosWSAsync(connectionString);
            _daoLog = new daoLogWSAsync(connectionString);
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }

        // Método privado para registrar errores en Log
        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método privado para registrar acciones exitosas en Bitácora
        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }


        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var permisos = await _daoPermiso.ObtenerPermisosAsync();
                await RegistrarBitacora("Vista Permisos", "Acceso exitoso a la lista de permisos");
                return View(permisos);
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de permisos", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                await RegistrarBitacora("Vista Crear Permiso", "Acceso a la vista de creación de permiso");
                return View();
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de permiso", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]

        public async Task<IActionResult> Crear(PermisoViewModel permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoPermiso.InsertarPermisoAsync(permiso);
                    await RegistrarBitacora("Crear Permiso", $"Permiso creado: {permiso.NombrePermiso}");
                    TempData["SuccessMessage"] = "Permiso creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarError("crear permiso", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]

        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var permiso = await _daoPermiso.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                await RegistrarBitacora("Ver Detalles Permiso", $"Detalles de permiso: {permiso.NombrePermiso} (ID: {id})");
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarError("ver detalles del permiso", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]

        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var permiso = await _daoPermiso.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                await RegistrarBitacora("Vista Editar Permiso", $"Acceso a edición de permiso: {permiso.NombrePermiso} (ID: {id})");
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la edición del permiso", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Editar(PermisoViewModel permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verificar si el permiso existe antes de actualizar
                    var permisoExistente = await _daoPermiso.ObtenerPermisoPorIdAsync(permiso.IdPermiso);
                    if (permisoExistente == null)
                    {
                        TempData["ErrorMessage"] = "El permiso no existe o ya fue eliminado.";
                        return RedirectToAction(nameof(Index));
                    }

                    await _daoPermiso.ActualizarPermisoAsync(permiso);
                    await RegistrarBitacora("Actualizar Permiso", $"Permiso actualizado: {permiso.NombrePermiso} (ID: {permiso.IdPermiso})");
                    TempData["SuccessMessage"] = "Permiso actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarError("actualizar permiso", ex);
                return View("Error");
            }
        }


        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]

        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var permiso = await _daoPermiso.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                await _daoPermiso.EliminarPermisoAsync(id);
                await RegistrarBitacora("Eliminar Permiso", $"Permiso eliminado: {permiso.NombrePermiso} (ID: {id})");

                TempData["SuccessMessage"] = "Permiso eliminado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar permiso", ex);
                TempData["ErrorMessage"] = "Error al eliminar el permiso.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
