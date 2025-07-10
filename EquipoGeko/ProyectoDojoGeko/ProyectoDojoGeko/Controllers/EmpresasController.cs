using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpresasController : Controller
    {
        private readonly daoEmpresaWSAsync _daoEmpresa;
        private readonly daoLogWSAsync _daoLog;
        private readonly IBitacoraService _bitacoraService;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;
        private readonly ILoggingService _loggingService;
        private readonly IEstadoService _estadoService;

        public EmpresasController(
            daoEmpresaWSAsync daoEmpresa,
            daoLogWSAsync daoLog,
            IBitacoraService bitacoraService,
            daoUsuariosRolWSAsync daoRolUsuario,
            ILoggingService loggingService,
            IEstadoService estadoService)
        {
            _daoEmpresa = daoEmpresa;
            _daoLog = daoLog;
            _bitacoraService = bitacoraService;
            _daoRolUsuario = daoRolUsuario;
            _loggingService = loggingService;
            _estadoService = estadoService;
        }

        // Método para registrar errores en el log
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

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        // Acción para mostrar la lista de empresas
        public async Task<IActionResult> Index()
        {
            // Intenta obtener la lista de empresas y registrar la acción en la bitácora
            try
            {
                // Obtiene la lista de empresas de forma asíncrona
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();
                // Registra la acción de acceso a la lista de empresas en la bitácora
                await _bitacoraService.RegistrarBitacoraAsync("Vista Empresas", "Acceso a la lista de empresas");
                // Devuelve la vista con la lista de empresas obtenida
                return View(empresas ?? new List<EmpresaViewModel>());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                // Registra el error en el log y devuelve una vista vacía
                await RegistrarError("acceder a la vista de empresas", ex);
                // Si ocurre un error, redirige a la vista de índice con una lista vacía
                return View(new List<EmpresaViewModel>());
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Acción para mostrar la vista de creación de una nueva empresa
        public async Task<IActionResult> Crear()
        {
            // Intenta acceder a la vista de creación de empresa y registrar la acción en la bitácora
            try
            {
                // Obtenemos los estados usando el servicio
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();

                await _bitacoraService.RegistrarBitacoraAsync("Vista Crear Empresa", "Acceso a la vista de creación de empresa");
                return View(new EmpresaViewModel());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de empresa", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        // Acción para crear una nueva empresa
        public async Task<IActionResult> Crear(EmpresaViewModel empresa)
        {
            // Intenta crear una nueva empresa y registrar la acción en la bitácora
            try
            {
                // Verifica si el modelo de la empresa es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    await RegistrarError("crear empresa - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(empresa);
                }
                // Inserta la nueva empresa en la base de datos de forma asíncrona
                var idEmpresa = await _daoEmpresa.InsertarEmpresaAsync(empresa);
                // Registra la acción de creación de la empresa en la bitácora
                await _bitacoraService.RegistrarBitacoraAsync("Crear Empresa", $"Empresa creada: {empresa.Nombre} (ID: {idEmpresa})");
                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Empresa creada correctamente";
                // Redirige al usuario a la lista de empresas después de la creación exitosa
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("crear empresa", ex);
                return View(empresa);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Acción para mostrar la vista de edición de una empresa existente
        public async Task<IActionResult> Editar(int id)
        {
            // Intenta obtener la empresa por ID y registrar la acción en la bitácora
            try
            {
                // Verifica si el ID de la empresa es válido
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                // Si la empresa no se encuentra, registra un error y devuelve NotFound
                if (empresa == null)
                {
                    await RegistrarError("editar empresa - no encontrada", new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }
                // Registra la acción de acceso a la vista de edición de la empresa en la bitácora
                await _bitacoraService.RegistrarBitacoraAsync("Vista Editar Empresa", $"Acceso a edición de empresa: {empresa.Nombre} (ID: {id})");
                // Devuelve la vista de edición con la empresa obtenida
                return View(empresa);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("obtener empresa para editar", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        // Acción para actualizar una empresa existente
        public async Task<IActionResult> Editar(EmpresaViewModel empresa)
        {
            // Intenta actualizar una empresa existente y registrar la acción en la bitácora
            try
            {
                // Verifica si el modelo de la empresa es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    await RegistrarError("actualizar empresa - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(empresa);
                }
                // Actualiza la empresa en la base de datos de forma asíncrona
                await _daoEmpresa.ActualizarEmpresaAsync(empresa);
                // Registra la acción de actualización de la empresa en la bitácora
                await _bitacoraService.RegistrarBitacoraAsync("Actualizar Empresa", $"Empresa actualizada: {empresa.Nombre} (ID: {empresa.IdEmpresa})");
                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Empresa actualizada correctamente";
                // Redirige al usuario a la lista de empresas después de la actualización exitosa
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("actualizar empresa", ex);
                return View(empresa);
            }
        }

        [HttpGet]
        // Acción para mostrar los detalles de una empresa específica
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Detalle(int id)
        {
            // Intenta obtener los detalles de la empresa por ID y registrar la acción en la bitácora
            try
            {
                // Verifica si el ID de la empresa es válido
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                {
                    await RegistrarError("ver detalles de empresa - no encontrada", new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }
                await _bitacoraService.RegistrarBitacoraAsync("Ver Detalles Empresa", $"Detalle de la empresa: {empresa.Nombre} (ID: {id})");
                return View(empresa);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("obtener detalles de empresa", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        // Acción para listar una empresa específica por ID
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Listar(int id)
        {
            // Intenta obtener la empresa por ID y registrar la acción en la bitácora
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                {
                    await RegistrarError("listar empresa - no encontrada", new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }
                await _bitacoraService.RegistrarBitacoraAsync("Listar Empresa", $"Empresa listada: {empresa.Nombre} (ID: {id})");
                return View(empresa);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("listar empresa", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        // Acción para eliminar una empresa existente
        public async Task<IActionResult> Eliminar(int id)
        {
            // Intenta eliminar una empresa existente y registrar la acción en la bitácora
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                {
                    await RegistrarError("eliminar empresa - no encontrada", new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }
                // Elimina la empresa de forma asíncrona
                await _daoEmpresa.EliminarEmpresaAsync(id);
                await _bitacoraService.RegistrarBitacoraAsync("Eliminar Empresa", $"Empresa eliminada: {empresa.Nombre} (ID: {id})");
                TempData["SuccessMessage"] = "Empresa eliminada correctamente";
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("eliminar empresa", ex);
                TempData["ErrorMessage"] = "Error al eliminar la empresa.";
                return RedirectToAction("Index");
            }
        }
    }
}