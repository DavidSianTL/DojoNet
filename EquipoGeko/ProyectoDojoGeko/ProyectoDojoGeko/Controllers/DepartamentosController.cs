using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class DepartamentosController : Controller
    {
        private readonly daoDepartamentoWSAsync _daoDepartamento; // DAO para manejar departamentos
        private readonly daoLogWSAsync _daoLog; // DAO para manejar logs
        private readonly daoBitacoraWSAsync _daoBitacora; // DAO para manejar bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario; // DAO para manejar roles de usuario

        // Constructor que inicializa los DAOs con la cadena de conexión
        public DepartamentosController()
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicialización de los DAOs
            _daoDepartamento = new daoDepartamentoWSAsync(connectionString);
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
        // Acción para mostrar la lista de departamentos
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor","Visualizador")]
        public async Task<IActionResult> Index()
        {
            // Intenta obtener la lista de departamentos y registrar la acción en la bitácora
            try
            {
                // Obtiene la lista de departamentos de forma asíncrona
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
                // Registra la acción de acceso a la lista de departamentos en la bitácora
                await RegistrarBitacora("Vista Departamentos", "Acceso a la lista de departamentos");
                // Devuelve la vista con la lista de departamentos obtenida
                return View(departamentos ?? new List<DepartamentoViewModel>());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                // Registra el error en el log y devuelve una vista vacía
                await RegistrarError("acceder a la vista de departamentos", ex);
                // Si ocurre un error, redirige a la vista de índice con una lista vacía
                return View(new List<DepartamentoViewModel>());
            }
        }

        [HttpGet]
        // Acción para mostrar la vista de creación de un nuevo departamento
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            // Intenta acceder a la vista de creación de departamento y registrar la acción en la bitácora
            try
            {
                await RegistrarBitacora("Vista Crear Departamento", "Acceso a la vista de creación de departamento");
                return View(new DepartamentoViewModel());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Acción para crear un nuevo departamento
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(DepartamentoViewModel departamento)
        {
            // Intenta crear un nuevo departamento y registrar la acción en la bitácora
            try
            {
                // Verifica si el modelo del departamento es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    await RegistrarError("crear departamento - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(departamento);
                }

                // Inserta el nuevo departamento en la base de datos de forma asíncrona
                await _daoDepartamento.InsertarDepartamentoAsync(departamento);
                // Registra la acción de creación del departamento en la bitácora
                await RegistrarBitacora("Crear Departamento", $"Departamento creado: {departamento.Nombre}");
                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Departamento creado correctamente";
                // Redirige al usuario a la lista de departamentos después de la creación exitosa
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("crear departamento", ex);
                return View(departamento);
            }
        }

        [HttpGet]
        // Acción para mostrar la vista de edición de un departamento existente
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Editar(int id)
        {
            // Intenta obtener el departamento por ID y registrar la acción en la bitácora
            try
            {
                // Verifica si el ID del departamento es válido
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                // Si el departamento no se encuentra, registra un error y devuelve NotFound
                if (departamento == null)
                {
                    await RegistrarError("editar departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }
                // Registra la acción de acceso a la vista de edición del departamento en la bitácora
                await RegistrarBitacora("Vista Editar Departamento", $"Acceso a edición de departamento: {departamento.Nombre} (ID: {id})");
                // Devuelve la vista de edición con el departamento obtenido
                return View(departamento);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("obtener departamento para editar", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Acción para actualizar un departamento existente
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Editar(DepartamentoViewModel departamento)
        {
            // Intenta actualizar un departamento existente y registrar la acción en la bitácora
            try
            {
                // Verifica si el modelo del departamento es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    await RegistrarError("actualizar departamento - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(departamento);
                }

                // Actualiza el departamento en la base de datos de forma asíncrona
                await _daoDepartamento.ActualizarDepartamentoAsync(departamento);
                // Registra la acción de actualización del departamento en la bitácora
                await RegistrarBitacora("Actualizar Departamento", $"Departamento actualizado: {departamento.Nombre} (ID: {departamento.IdDepartamento})");
                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Departamento actualizado correctamente";
                // Redirige al usuario a la lista de departamentos después de la actualización exitosa
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("actualizar departamento", ex);
                return View(departamento);
            }
        }

        [HttpGet]
        // Acción para mostrar los detalles de un departamento específico
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Detalle(int id)
        {
            // Intenta obtener los detalles del departamento por ID y registrar la acción en la bitácora
            try
            {
                // Verifica si el ID del departamento es válido
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("ver detalles de departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Ver Detalles Departamento", $"Detalle del departamento: {departamento.Nombre} (ID: {id})");
                return View(departamento);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("obtener detalles de departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        // Acción para listar un departamento específico por ID
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Listar(int id)
        {
            // Intenta obtener el departamento por ID y registrar la acción en la bitácora
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("listar departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Listar Departamento", $"Departamento listado: {departamento.Nombre} (ID: {id})");
                return View(departamento);
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("listar departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Acción para eliminar un departamento existente
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Eliminar(int id)
        {
            // Intenta eliminar un departamento existente y registrar la acción en la bitácora
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("eliminar departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }

                // Elimina el departamento de forma asíncrona
                await _daoDepartamento.EliminarDepartamentoAsync(id);
                await RegistrarBitacora("Eliminar Departamento", $"Departamento eliminado: {departamento.Nombre} (ID: {id})");

                TempData["SuccessMessage"] = "Departamento eliminado correctamente";
                return RedirectToAction("Index");
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("eliminar departamento", ex);
                TempData["ErrorMessage"] = "Error al eliminar el departamento.";
                return RedirectToAction("Index");
            }
        }
    }
}