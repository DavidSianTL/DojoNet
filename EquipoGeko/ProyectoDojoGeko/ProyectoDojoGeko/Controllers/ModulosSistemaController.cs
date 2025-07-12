using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services;

namespace ProyectoDojoGeko.Controllers
{
    public class ModulosSistemaController : Controller
    {
        private readonly daoModuloSistema _daoModuloSistema;
        private readonly daoModulo _daoModulo;
        private readonly IEstadoService _estadoService;

        public ModulosSistemaController(daoModuloSistema daoModuloSistema, daoModulo daoModulo, IEstadoService estadoService)
        {
            _daoModuloSistema = daoModuloSistema;
            _daoModulo = daoModulo;
            _estadoService = estadoService;
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> Index(int idSistema)
        {
            var modulosSistema = await _daoModuloSistema.ObtenerModulosPorSistemaAsync(idSistema);
            return View(modulosSistema);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Modulos = await _daoModulo.ObtenerModulosAsync();
            ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(ModuloSistemaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Modulos = await _daoModulo.ObtenerModulosAsync();
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();
                return View(model);
            }
            await _daoModuloSistema.InsertarModuloSistemaAsync(model);
            TempData["Success"] = "Módulo asignado correctamente al sistema.";
            return RedirectToAction("Crear");
        }

        
    }
}
