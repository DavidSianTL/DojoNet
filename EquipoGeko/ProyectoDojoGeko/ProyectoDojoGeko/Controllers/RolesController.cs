using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class RolesController : Controller
    {
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly ILoggingService _loggingService;

        public RolesController(
            daoRolesWSAsync daoRoles,
            daoUsuariosRolWSAsync daoRolUsuario,
            daoBitacoraWSAsync daoBitacora,
            ILoggingService loggingService)
        {
            _daoRoles = daoRoles;
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

        private async Task RegistrarBitacora(string accion, string descripcionExtra)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcionExtra} (Acción realizada por {usuario})",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _daoRoles.ObtenerRolesAsync();
                await RegistrarBitacora("Vista Roles", "Ingreso a la vista de roles");
                return View(roles);
            }
            catch (Exception ex)
            {
                await RegistrarError("cargar listado de roles", ex);
                return View("Error");
            }
        }
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                await RegistrarBitacora("Vista Crear Rol", "Acceso a la vista de creación de rol");
                return View();
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de rol", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(RolesViewModel rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoRoles.InsertarRolAsync(rol);
                    await RegistrarBitacora("Crear Rol", $"Se ha creado el rol '{rol.NombreRol}'");
                    TempData["SuccessMessage"] = "Rol creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("crear rol", ex);
                return View("Error");
            }
        }
        [HttpPost]
        [ActionName("Crear")]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> CrearPost(RolesViewModel rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoRoles.InsertarRolAsync(rol);
                    await RegistrarBitacora("Crear Rol", $"Se ha creado el rol '{rol.NombreRol}'");
                    TempData["SuccessMessage"] = "Rol creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("crear rol", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Editar(RolesViewModel rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rolExistente = await _daoRoles.ObtenerRolPorIdAsync(rol.IdRol);
                    if (rolExistente == null)
                    {
                        TempData["ErrorMessage"] = "El rol no existe o ya fue eliminado.";
                        return RedirectToAction(nameof(Index));
                    }

                    await _daoRoles.ActualizarRolAsync(rol);
                    await RegistrarBitacora("Editar Rol", $"Se ha editado el rol '{rol.NombreRol}'");
                    TempData["SuccessMessage"] = "Rol actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("editar rol", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                await RegistrarBitacora("Vista Eliminar Rol", $"Acceso a eliminación del rol: {rol.NombreRol} (ID: {id})");
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("cargar vista de eliminación de rol", ex);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                var rolExistente = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rolExistente == null)
                {
                    TempData["ErrorMessage"] = "El rol ya no existe o ya fue eliminado.";
                    return RedirectToAction(nameof(Index));
                }

                await _daoRoles.DesactivarRolAsync(id);
                await RegistrarBitacora("Eliminar Rol", $"Rol con ID {id} desactivado");
                TempData["SuccessMessage"] = "Rol eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar rol", ex);
                return View("Error");
            }
        }
    }
}

