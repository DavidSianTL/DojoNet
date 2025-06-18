using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class RolesController : Controller
    {
        private readonly daoRolesWSAsync _daoRoles; // DAO para manejar roles
        private readonly daoLogWSAsync _daoLog; //  DAO para manejar logs
        private readonly daoBitacoraWSAsync _daoBitacora; // DAO para manejar bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario; // DAO para manejar la relación entre usuarios y roles

        public RolesController()
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicialización de los DAOs
            _daoRoles = new daoRolesWSAsync(connectionString);
            // Inicialización de los DAOs
            _daoLog = new daoLogWSAsync(connectionString);
            // Inicialización de los DAOs
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
            // Inicialización de los DAOs
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }
        // Método para registrar errores en el log
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

        // Método para registrar acciones en la bitácora
        private async Task RegistrarBitacora(string accion, string descripcionExtra)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0; // ID del usuario actual
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema"; // Nombre del usuario actual
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0; // ID del sistema actual
            // Descripción de la acción realizada
            string descripcion = $"{descripcionExtra} (Acción realizada por {usuario})";
            // Inserción de la bitácora en la base de datos
            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        // Método para mostrar la lista de roles
        public async Task<IActionResult> Index()
        {
            // Método para cargar la vista de roles
            try
            {
                // Obtiene la lista de roles desde el DAO
                var roles = await _daoRoles.ObtenerRolesAsync();
                // Registra la acción en la bitácora
                await RegistrarBitacora("Vista Roles", "Ingreso a la vista de roles");
                // Retorna la vista con la lista de roles
                return View(roles);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                // Registra el error en el log
                await RegistrarError("cargar listado de roles", ex);
                // Retorna la vista de error
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para mostrar la vista de creación de un nuevo rol
        public async Task<IActionResult> Crear()
        {
            // Método para cargar la vista de creación de rol
            try
            {
                // Registra la acción en la bitácora
                await RegistrarBitacora("Vista Crear Rol", "Acceso a la vista de creación de rol");
                // Retorna la vista de creación de rol
                return View();
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                // Registra el error en el log
                await RegistrarError("acceder a la vista de creación de rol", ex);
                // Retorna la vista de error
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para crear un nuevo rol
        public async Task<IActionResult> Crear(RolesViewModel rol)
        {
            // Método para procesar la creación de un nuevo rol
            try
            {
                // Verifica si el modelo es válido
                if (ModelState.IsValid)
                {
                    // Verifica si el rol ya existe
                    await _daoRoles.InsertarRolAsync(rol);
                    // Registra la acción en la bitácora
                    await RegistrarBitacora("Crear Rol", $"Se ha creado el rol '{rol.NombreRol}'");
                    // Muestra un mensaje de éxito al usuario
                    TempData["SuccessMessage"] = "Rol creado correctamente.";
                    // Redirige a la lista de roles
                    return RedirectToAction(nameof(Index));
                }
                // Si el modelo no es válido, retorna la vista con los datos ingresados
                return View(rol);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("crear rol", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para mostrar los detalles de un rol específico
        public async Task<IActionResult> Detalles(int id)
        {
            // Método para cargar los detalles de un rol
            try
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                await RegistrarBitacora("Ver Detalles Rol", $"Visualización de detalles del rol '{rol.NombreRol}' (ID: {id})");
                return View(rol);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("ver detalles del rol", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para mostrar la vista de edición de un rol específico
        public async Task<IActionResult> Editar(int id)
        {
            // Método para cargar la vista de edición de un rol
            try
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                await RegistrarBitacora("Vista Editar Rol", $"Acceso a edición del rol: {rol.NombreRol} (ID: {id})");
                return View(rol);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("cargar vista de edición del rol", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para actualizar un rol existente
        public async Task<IActionResult> Editar(RolesViewModel rol)
        {
            // Método para procesar la actualización de un rol existente
            try
            {
                // Verifica si el modelo es válido
                if (ModelState.IsValid)
                {
                    var rolExistente = await _daoRoles.ObtenerRolPorIdAsync(rol.IdRol); // Obtiene el rol existente por ID
                    if (rolExistente == null) // verifica si el rol existe
                    {
                        TempData["ErrorMessage"] = "El rol no existe o ya fue eliminado."; // Si no existe, muestra un mensaje de error
                        return RedirectToAction(nameof(Index)); // Redirige a la lista de roles
                    }
                    // Actualiza el rol existente con los nuevos datos
                    await _daoRoles.ActualizarRolAsync(rol);
                    await RegistrarBitacora("Editar Rol", $"Se ha editado el rol '{rol.NombreRol}'");
                    TempData["SuccessMessage"] = "Rol actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(rol);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                // Registra el error en el log
                await RegistrarError("editar rol", ex);
                return View("Error");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para mostrar la vista de eliminación de un rol específico
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            // Método para cargar la vista de eliminación de un rol
            {
                var rol = await _daoRoles.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                await RegistrarBitacora("Vista Eliminar Rol", $"Acceso a eliminación del rol: {rol.NombreRol} (ID: {id})");
                return View(rol);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("cargar vista de eliminación de rol", ex);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para confirmar la eliminación de un rol
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            // Método para procesar la eliminación de un rol
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
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("eliminar rol", ex);
                return View("Error");
            }
        }
    }
}
