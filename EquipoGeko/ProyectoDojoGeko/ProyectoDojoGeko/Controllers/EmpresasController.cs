using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpresasController : Controller
    {
        private readonly daoEmpresaWSAsync _daoEmpresa; // DAO para manejar empresas
        private readonly daoLogWSAsync _daoLog; // DAO para manejar logs
        private readonly daoBitacoraWSAsync _daoBitacora; // DAO para manejar bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario; // DAO para manejar roles de usuario

        // Constructor que inicializa los DAOs con la cadena de conexión
        public EmpresasController()
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicialización de los DAOs
            _daoEmpresa = new daoEmpresaWSAsync(connectionString);
            _daoLog = new daoLogWSAsync(connectionString);
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }

        // Método para registrar errores en el log
        private async Task RegistrarError(string accion, Exception ex)
        {
            // Verifica si la sesión contiene el nombre de usuario
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            // Inserta el error en el log
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
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
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            // Inserta una nueva entrada en la bitácora
            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
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
                await RegistrarBitacora("Vista Empresas", "Acceso a la lista de empresas");
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
                await RegistrarBitacora("Vista Crear Empresa", "Acceso a la vista de creación de empresa");
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
                await RegistrarBitacora("Crear Empresa", $"Empresa creada: {empresa.Nombre} (ID: {idEmpresa})");
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
                await RegistrarBitacora("Vista Editar Empresa", $"Acceso a edición de empresa: {empresa.Nombre} (ID: {id})");
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
                await RegistrarBitacora("Actualizar Empresa", $"Empresa actualizada: {empresa.Nombre} (ID: {empresa.IdEmpresa})");
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

                await RegistrarBitacora("Ver Detalles Empresa", $"Detalle de la empresa: {empresa.Nombre} (ID: {id})");
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

                await RegistrarBitacora("Listar Empresa", $"Empresa listada: {empresa.Nombre} (ID: {id})");
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
                await RegistrarBitacora("Eliminar Empresa", $"Empresa eliminada: {empresa.Nombre} (ID: {id})");

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