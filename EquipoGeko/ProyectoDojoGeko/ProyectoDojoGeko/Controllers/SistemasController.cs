using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemasController : Controller
    {
        private readonly daoSistemaWSAsync _daoSistema;// DAO para manejar sistemas
        private readonly daoLogWSAsync _daoLog; // DAO para manejar logs
        private readonly daoBitacoraWSAsync _daoBitacora; // DAO para manejar bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario; // DAO para manejar roles de usuario

        // Constructor que inicializa los DAOs con la cadena de conexión
        public SistemasController()
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicialización de los DAOs
            _daoSistema = new daoSistemaWSAsync(connectionString);
            // Inicialización de los DAOs para logs, bitácoras y roles de usuario
            _daoLog = new daoLogWSAsync(connectionString);
            // Inicialización de los DAOs para bitácoras y roles de usuario
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
            // Inicialización del DAO para manejar roles de usuario
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }

        // Método para registrar errores en el log
        private async Task RegistrarError(string accion, Exception ex)
        {
            // Verifica si la sesión contiene el ID de usuario y el nombre de usuario
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            // Obtiene el ID del sistema desde la sesión
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                // FechaEntrada = DateTime.Now,
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método para registrar acciones en la bitácora
        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            // Obtiene el ID de usuario, nombre de usuario y ID del sistema desde la sesión
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            // Obtiene el nombre de usuario y el ID del sistema desde la sesión, o asigna valores por defecto
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            // Obtiene el ID del sistema desde la sesión, o asigna 0 si no está disponible
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            // Inserta una nueva entrada en la bitácora con la acción y descripción proporcionadas
            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                // FechaEntrada = DateTime.Now,
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador","Editor","Visualizador")]
        // Acción para mostrar la lista de sistemas
        public async Task<IActionResult> Index()
        {
            // Intenta obtener la lista de sistemas y registrar la acción en la bitácora
            try
            {
                // Obtiene la lista de sistemas de forma asíncrona
                var sistemas = await _daoSistema.ObtenerSistemasAsync();
                // Registra la acción de acceso a la lista de sistemas en la bitácora
                await RegistrarBitacora("Vista Sistemas", "Acceso a la lista de sistemas");
                // Devuelve la vista con la lista de sistemas obtenida
                return View(sistemas ?? new List<SistemaViewModel>());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                // Registra el error en el log y devuelve una vista vacía
                await RegistrarError("acceder a la vista de sistemas", ex);
                // Si ocurre un error, redirige a la vista de índice con una lista vacía
                return View(new List<SistemaViewModel>());
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Acción para mostrar la vista de creación de un nuevo sistema
        public async Task<IActionResult> Crear()
        {
            // Intenta acceder a la vista de creación de sistema y registrar la acción en la bitácora
            try
            {
                await RegistrarBitacora("Vista Crear Sistema", "Acceso a la vista de creación de sistema");
                return View(new SistemaViewModel());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de sistema", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        // Acción para crear un nuevo sistema
        public async Task<IActionResult> Crear(SistemaViewModel sistema)
        {
            // Intenta crear un nuevo sistema y registrar la acción en la bitácora
            try
            {
                // Verifica si el modelo del sistema es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    await RegistrarError("crear sistema - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(sistema);
                }
                // Inserta el nuevo sistema en la base de datos de forma asíncrona
                await _daoSistema.InsertarSistemaAsync(sistema);
                // Registra la acción de creación del sistema en la bitácora
                await RegistrarBitacora("Crear Sistema", $"Sistema creado: {sistema.Nombre}");
                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Sistema creado correctamente";
                // Redirige al usuario a la lista de sistemas después de la creación exitosa
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("crear sistema", ex);
                return View(sistema);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Acción para mostrar la vista de edición de un sistema existente
        public async Task<IActionResult> Editar(int id)
        {
            // Intenta obtener el sistema por ID y registrar la acción en la bitácora
            try
            {
                // Verifica si el ID del sistema es válido
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                // Si el sistema no se encuentra, registra un error y devuelve NotFound
                if (sistema == null)
                {
                    // Registra un error en el log y devuelve NotFound
                    await RegistrarError("editar sistema - no encontrado", new Exception($"Sistema con ID {id} no encontrado"));
                    // devuelve una respuesta NotFound si el sistema no existe
                    return NotFound();
                }
                // Registra la acción de acceso a la vista de edición del sistema en la bitácora
                await RegistrarBitacora("Vista Editar Sistema", $"Acceso a edición de sistema: {sistema.Nombre} (ID: {id})");
                // Devuelve la vista de edición con el sistema obtenido
                return View(sistema);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("obtener sistema para editar", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        // Acción para actualizar un sistema existente
        public async Task<IActionResult> Editar(SistemaViewModel sistema)
        {
            // Intenta actualizar un sistema existente y registrar la acción en la bitácora
            try
            {
                // Verifica si el modelo del sistema es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    // Registra un error si el modelo no es válido y devuelve la vista con el modelo
                    await RegistrarError("actualizar sistema - datos inválidos", new Exception("Validación de modelo fallida"));
                    // devuelve la vista de edición con el modelo no válido
                    return View(sistema);
                }
                // Actualiza el sistema en la base de datos de forma asíncrona
                await _daoSistema.ActualizarSistemaAsync(sistema);
                // Registra la acción de actualización del sistema en la bitácora
                await RegistrarBitacora("Actualizar Sistema", $"Sistema actualizado: {sistema.Nombre} (ID: {sistema.IdSistema})");
                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Sistema actualizado correctamente";
                // Redirige al usuario a la lista de sistemas después de la actualización exitosa
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("actualizar sistema", ex);
                return View(sistema);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Acción para mostrar los detalles de un sistema específico
        public async Task<IActionResult> Detalle(int id)
        {
            // Intenta obtener los detalles del sistema por ID y registrar la acción en la bitácora
            try
            {
                // Verifica si el ID del sistema es válido
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("ver detalles de sistema - no encontrado", new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Ver Detalles Sistema", $"Detalle del sistema: {sistema.Nombre} (ID: {id})");
                return View(sistema);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("obtener detalles de sistema", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Acción para listar un sistema específico por ID
        public async Task<IActionResult> Listar(int id)
        {
            // Intenta obtener el sistema por ID y registrar la acción en la bitácora
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("listar sistema - no encontrado", new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Listar Sistema", $"Sistema listado: {sistema.Nombre} (ID: {id})");
                return View(sistema);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("listar sistema", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        // Acción para eliminar un sistema existente
        public async Task<IActionResult> Eliminar(int id)
        {
            // Intenta eliminar un sistema existente y registrar la acción en la bitácora
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("eliminar sistema - no encontrado", new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }
                // Elimina el sistema de forma asíncrona
                await _daoSistema.EliminarSistemaAsync(id);
                await RegistrarBitacora("Eliminar Sistema", $"Sistema eliminado: {sistema.Nombre} (ID: {id})");

                TempData["SuccessMessage"] = "Sistema eliminado correctamente";
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("eliminar sistema", ex);
                TempData["ErrorMessage"] = "Error al eliminar el sistema.";
                return RedirectToAction("Index");
            }
        }
    }
}
