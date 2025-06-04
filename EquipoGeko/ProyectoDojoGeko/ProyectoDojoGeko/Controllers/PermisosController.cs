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

        private async Task RegistrarLogYBitacora(string accion, string descripcion)
        {
            try
            {
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = accion,
                    Descripcion = descripcion,
                    Estado = true
                });

                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);
                var idSistema = rolesUsuario.FirstOrDefault()?.FK_IdSistema ?? 0;

                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = accion,
                    Descripcion = descripcion,
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });
            }
            catch
            {
                // Ignorar errores al registrar bitácora/logs
            }
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
                await RegistrarLogYBitacora("Error Index Permisos", ex.Message);
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
                    await RegistrarLogYBitacora("Crear Permiso", $"Permiso '{permiso.NombrePermiso}' creado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Crear Permiso", ex.Message);
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
                await RegistrarLogYBitacora("Error Detalles Permiso", ex.Message);
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
                await RegistrarLogYBitacora("Error Editar Permiso (GET)", ex.Message);
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
                    await RegistrarLogYBitacora("Editar Permiso", $"Permiso '{permiso.NombrePermiso}' actualizado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Permiso (POST)", ex.Message);
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
                await RegistrarLogYBitacora("Error Eliminar Permiso (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarPermisoAsync(id);
                await RegistrarLogYBitacora("Eliminar Permiso", $"Permiso con ID {id} desactivado.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Permiso (POST)", ex.Message);
                return View("Error");
            }
        }
    }
}
