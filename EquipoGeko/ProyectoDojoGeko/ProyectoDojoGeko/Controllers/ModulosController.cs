using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class ModulosController : Controller
    {
        private readonly daoModulo _daoModulo;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly IBitacoraService _bitacoraService;
        private readonly ILoggingService _loggingService;
        private readonly IEstadoService _estadoService;

        public ModulosController(
            daoModulo daoModulo,
            daoLogWSAsync daoLog,
            daoSistemaWSAsync daoSistema,
            IBitacoraService bitacoraService,
            ILoggingService loggingService,
            IEstadoService estadoService)
        {
            _daoModulo = daoModulo;
            _daoLog = daoLog;
            _daoSistema = daoSistema;
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
                var modulos = await _daoModulo.ObtenerModulosAsync();
                await _bitacoraService.RegistrarBitacoraAsync("Vista Módulos", "Acceso a la lista de módulos");

                // Obtenemos los estados usando el servicio
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();

                return View(modulos ?? new List<ModuloViewModel>());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de módulos", ex);
                return View(new List<ModuloViewModel>());
            }
        }

        // Crear un nuevo módulo
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                // Obtenemos los sistemas usando el servicio
                ViewBag.Sistemas = await _daoSistema.ObtenerSistemasAsync();

                // Obtenemos los estados usando el servicio
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();

                await _bitacoraService.RegistrarBitacoraAsync("Vista Módulos", "Acceso a la lista de módulos");

                return View(new ModuloViewModel());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de módulos", ex);
                return View(new ModuloViewModel());
            }
        }
        
    }
}