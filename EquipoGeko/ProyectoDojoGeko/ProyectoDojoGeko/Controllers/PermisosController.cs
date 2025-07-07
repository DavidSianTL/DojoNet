using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class PermisosController : Controller
    {
        private readonly daoPermisosWSAsync _daoPermiso;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly ILoggingService _loggingService;

        public PermisosController(
            daoPermisosWSAsync daoPermiso,
            daoUsuariosRolWSAsync daoRolUsuario,
            daoBitacoraWSAsync daoBitacora,
            ILoggingService loggingService)
        {
            _daoPermiso = daoPermiso;
            _daoRolUsuario = daoRolUsuario;
            _daoBitacora = daoBitacora;
            _loggingService = loggingService;
        }

        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _loggingService.RegistrarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }
        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var permiso = await _daoPermiso.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                {
                    TempData["ErrorMessage"] = "El permiso ya no existe.";
                    return RedirectToAction(nameof(Index));
                }

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
