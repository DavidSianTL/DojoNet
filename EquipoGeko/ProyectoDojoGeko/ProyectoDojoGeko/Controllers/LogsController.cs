using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class LogsController : Controller
    {
        private readonly ILoggingService _loggingService;

        public LogsController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // Acción que muestra la vista de usuarios
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> Index()
        {
            var logs = await _loggingService.ObtenerLogsAsync();
            return View(logs);
        }

        // Acción para guardar un nuevo log
        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador")]
        public async Task<IActionResult> GuardarLog(LogViewModel log)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _loggingService.RegistrarLogAsync(log);
                    return RedirectToAction(nameof(Index));
                }

                return View(log);
            }
            catch (Exception e)
            {
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error al Guardar manualmente un Log",
                    Descripcion = $"Error al guardar el log: {e.Message}",
                    Estado = false
                });

                ViewBag.ErrorMessage = "Ocurrió un error al guardar el log. Por favor, inténtelo de nuevo.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
