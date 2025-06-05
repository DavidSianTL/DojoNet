using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class RolesController : Controller
    {
        private readonly daoRolesWSAsync _daoRoles; // Acceso a datos de roles
        private readonly daoLogWSAsync _daoLog; //  Acceso a datos de logs
        private readonly daoBitacoraWSAsync _daoBitacora; //    Acceso a datos de bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario; // Acceso a datos de roles de usuario

        public RolesController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoRoles = new daoRolesWSAsync(connectionString); // Acceso a datos de roles
            _daoLog = new daoLogWSAsync(connectionString); // Acceso a datos de logs
            _daoBitacora = new daoBitacoraWSAsync(connectionString); // Acceso a datos de bitácoras
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString); // Acceso a datos de roles de usuario
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
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0; // Obtiene el ID del usuario de la sesión
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema"; // Obtiene el nombre del usuario de la sesión
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0; // Obtiene el ID del sistema de la sesión

            // Inserta una nueva entrada en la bitácora
            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        [AuthorizeRole("SuperAdmin")]
        // Método para cargar la lista de roles
        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _daoRoles.ObtenerRolesAsync();
                return View(roles);
            }
            catch (Exception ex)
            {
                await RegistrarError("cargar listado de roles", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Método para acceder a la vista de creación de rol
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
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Método para crear un nuevo rol
        public async Task<IActionResult> Crear(RolesViewModel rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoRoles.InsertarRolAsync(rol);
                    await RegistrarBitacora("Crear Rol", $"Rol '{rol.NombreRol}' creado.");
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


        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Método para ver los detalles de un rol específico
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("ver detalles del rol", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Método para acceder a la vista de edición de un rol
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                await RegistrarBitacora("Vista Editar Rol", $"Acceso a edición del rol: {rol.NombreRol} (ID: {id})");
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("cargar vista de edición del rol", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Método para actualizar un rol existente
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
                    await RegistrarBitacora("Editar Rol", $"Rol '{rol.NombreRol}' actualizado.");
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
        [AuthorizeRole("SuperAdmin")]
        // Método para acceder a la vista de eliminación de un rol
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarError("cargar vista de eliminación de rol", ex);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        [AuthorizeRole("SuperAdmin")]
        // Método para confirmar la eliminación de un rol
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
                await RegistrarBitacora("Eliminar Rol", $"Rol con ID {id} desactivado.");
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
