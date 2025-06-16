using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class PermisosController : Controller
    {
        private readonly daoPermisosWSAsync _daoPermiso; // DAO para manejar roles
        private readonly daoLogWSAsync _daoLog; // DAO para manejar logs
        private readonly daoBitacoraWSAsync _daoBitacora; // DAO para manejar bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario; // DAO para manejar usuarios y roles

        public PermisosController()
        {
            // Configuración de conexión a la base de datos
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoPermiso = new daoPermisosWSAsync(connectionString); // DAO para manejar permisos
            _daoLog = new daoLogWSAsync(connectionString);// DAO para manejar logs
            _daoBitacora = new daoBitacoraWSAsync(connectionString);// DAO para manejar bitácoras
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);// DAO para manejar usuarios y roles
        }

        // Métodos privados para registrar errores y bitácoras
        private async Task RegistrarError(string accion, Exception ex)
        {
            // Registra el error en la bitácora
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            // Registra el error en el log
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método para registrar acciones en la bitácora
        private async Task RegistrarBitacora(string accion, string descripcionExtra)
        {
            // Obtiene el ID del usuario y el nombre de usuario desde la sesión
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            // Obtiene el nombre de usuario y el ID del sistema desde la sesión
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            // Obtiene el ID del sistema desde la sesión, o usa 0 si no está disponible
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            // Crea la descripción de la acción realizada
            string descripcion = $"{descripcionExtra} (Acción realizada por {usuario})";
            // Inserta la acción en la bitácora
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
        // Acción para mostrar la lista de permisos
        public async Task<IActionResult> Index()
        {
            // Intenta obtener la lista de permisos y registrar la acción en la bitácora
            try
            // Obtener la lista de permisos desde el DAO
            {
                var permisos = await _daoPermiso.ObtenerPermisosAsync();
                await RegistrarBitacora("Vista Permisos", "Acceso exitoso a la lista de permisos");
                return View(permisos);
            }
            // Si ocurre un error, registrar el error y mostrar la vista de error
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de permisos", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Acción para mostrar la vista de creación de un nuevo permiso
        public async Task<IActionResult> Crear()
        {
            // Intenta acceder a la vista de creación de permiso y registrar la acción en la bitácora
            try
            {
                await RegistrarBitacora("Vista Crear Permiso", "Acceso a la vista de creación de permiso");
                return View();
            }
            // Si ocurre un error, registrar el error y mostrar la vista de error
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de permiso", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Acción para crear un nuevo permiso
        public async Task<IActionResult> Crear(PermisoViewModel permiso)
        {
            // Intenta crear un nuevo permiso y registrar la acción en la bitácora
            try
            {
                if (ModelState.IsValid)
                {
                    permiso.Estado = true; // Asigna el estado activo por defecto
                    await _daoPermiso.InsertarPermisoAsync(permiso);
                    await RegistrarBitacora("Crear Permiso", $"Permiso creado: {permiso.NombrePermiso}");
                    TempData["SuccessMessage"] = "Permiso creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            // Si ocurre un error, registrar el error y mostrar la vista de error
            catch (Exception ex)
            {
                await RegistrarError("crear permiso", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Acción para ver los detalles de un permiso específico
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var permiso = await _daoPermiso.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                await RegistrarBitacora("Ver Detalles Permiso", $"Visualización de detalles del permiso: {permiso.NombrePermiso} (ID: {id})");
                return View(permiso);
            }
            // Si ocurre un error, registrar el error y mostrar la vista de error
            catch (Exception ex)
            {
                await RegistrarError("ver detalles del permiso", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Acción para editar un permiso existente
        public async Task<IActionResult> Editar(int id)
        {
            // Intenta acceder a la vista de edición de un permiso y registrar la acción en la bitácora
            try
            {
                var permiso = await _daoPermiso.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                await RegistrarBitacora("Vista Editar Permiso", $"Acceso a edición de permiso: {permiso.NombrePermiso} (ID: {id})");
                return View(permiso);
            }
            // Si ocurre un error, registrar el error y mostrar la vista de error
            catch (Exception ex)
            {
                await RegistrarError("acceder a la edición del permiso", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        // Acción para actualizar un permiso existente
        public async Task<IActionResult> Editar(PermisoViewModel permiso)
        {
            // Intenta actualizar un permiso existente y registrar la acción en la bitácora
            try
            {
                if (ModelState.IsValid)
                {
                    var permisoExistente = await _daoPermiso.ObtenerPermisoPorIdAsync(permiso.IdPermiso);
                    if (permisoExistente == null)
                    {
                        TempData["ErrorMessage"] = "El permiso no existe o ya fue eliminado.";
                        return RedirectToAction(nameof(Index));
                    }
                    // Actualiza el permiso en la base de datos
                    await _daoPermiso.ActualizarPermisoAsync(permiso);
                    // Registra la acción en la bitácora
                    await RegistrarBitacora("Actualizar Permiso", $"Permiso actualizado: {permiso.NombrePermiso} (ID: {permiso.IdPermiso})");
                    // Muestra un mensaje de éxito
                    TempData["SuccessMessage"] = "Permiso actualizado correctamente";
                    // Redirige a la lista de permisos
                    return RedirectToAction(nameof(Index));
                }

                return View(permiso);
            }
            // Si ocurre un error, registrar el error y mostrar la vista de error
            catch (Exception ex)
            {
                await RegistrarError("actualizar permiso", ex);
                return View("Error");
            }
        }


        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
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
