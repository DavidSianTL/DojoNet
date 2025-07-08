using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class DepartamentosController : Controller
    {
        private readonly daoDepartamentoWSAsync _daoDepartamento;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILoggingService _loggingService;
        private readonly IEstadoService _estadoService;

        public DepartamentosController(
            daoDepartamentoWSAsync daoDepartamento,
            daoUsuariosRolWSAsync daoRolUsuario,
            IBitacoraService bitacoraService,
            ILoggingService loggingService,
            IEstadoService estadoService)
        {
            _daoDepartamento = daoDepartamento;
            _daoRolUsuario = daoRolUsuario;
            _bitacoraService = bitacoraService;
            _loggingService = loggingService;
            _estadoService = estadoService;
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

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
                await _bitacoraService.RegistrarBitacoraAsync("Vista Departamentos", "Acceso a la lista de departamentos");
                return View(departamentos ?? new List<DepartamentoViewModel>());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de departamentos", ex);
                return View(new List<DepartamentoViewModel>());
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                await _bitacoraService.RegistrarBitacoraAsync("Vista Crear Departamento", "Acceso a la vista de creación de departamento");
                return View(new DepartamentoViewModel());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(DepartamentoViewModel departamento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RegistrarError("crear departamento - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(departamento);
                }

                await _daoDepartamento.InsertarDepartamentoAsync(departamento);
                await _bitacoraService.RegistrarBitacoraAsync("Crear Departamento", $"Departamento creado: {departamento.Nombre}");
                TempData["SuccessMessage"] = "Departamento creado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("crear departamento", ex);
                return View(departamento);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("editar departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }
                await _bitacoraService.RegistrarBitacoraAsync("Vista Editar Departamento", $"Acceso a edición de departamento: {departamento.Nombre} (ID: {id})");
                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarError("obtener departamento para editar", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Editar(DepartamentoViewModel departamento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RegistrarError("actualizar departamento - datos inválidos", new Exception("Validación de modelo fallida"));
                    return View(departamento);
                }

                await _daoDepartamento.ActualizarDepartamentoAsync(departamento);
                await _bitacoraService.RegistrarBitacoraAsync("Actualizar Departamento", $"Departamento actualizado: {departamento.Nombre} (ID: {departamento.IdDepartamento})");
                TempData["SuccessMessage"] = "Departamento actualizado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("actualizar departamento", ex);
                return View(departamento);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("ver detalles de departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }

                await _bitacoraService.RegistrarBitacoraAsync("Ver Detalles Departamento", $"Detalle del departamento: {departamento.Nombre} (ID: {id})");
                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarError("obtener detalles de departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Listar(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("listar departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }

                await _bitacoraService.RegistrarBitacoraAsync("Listar Departamento", $"Departamento listado: {departamento.Nombre} (ID: {id})");
                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarError("listar departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                {
                    await RegistrarError("eliminar departamento - no encontrado", new Exception($"Departamento con ID {id} no encontrado"));
                    return NotFound();
                }

                await _daoDepartamento.EliminarDepartamentoAsync(id);
                await _bitacoraService.RegistrarBitacoraAsync("Eliminar Departamento", $"Departamento eliminado: {departamento.Nombre} (ID: {id})");

                TempData["SuccessMessage"] = "Departamento eliminado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar departamento", ex);
                TempData["ErrorMessage"] = "Error al eliminar el departamento.";
                return RedirectToAction("Index");
            }
        }
    }
}