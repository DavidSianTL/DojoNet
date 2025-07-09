using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.RolPermisos;
using ProyectoDojoGeko.Models.SistemasEmpresa;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemasEmpresaController : Controller
    {
        private readonly daoSistemasEmpresaWSAsync _daoSistemasEmpresa;
        private readonly daoSistemaWSAsync _daoSistemas;
        private readonly daoEmpresaWSAsync _daoEmpresas;
        private readonly IBitacoraService _bitacoraService;
        private readonly daoLogWSAsync _daoLog;
        private readonly ILoggingService _loggingService;

        public SistemasEmpresaController(
            daoSistemasEmpresaWSAsync daoSistemasEmpresa,
            daoSistemaWSAsync daoSistemas,
            daoEmpresaWSAsync daoEmpresas,
            IBitacoraService bitacoraService,
            daoLogWSAsync daoLog,
            ILoggingService loggingService)
        {
            _daoSistemasEmpresa = daoSistemasEmpresa;
            _daoSistemas = daoSistemas;
            _daoEmpresas = daoEmpresas;
            _bitacoraService = bitacoraService;
            _daoLog = daoLog;
            _loggingService = loggingService;
        }

        // Método privado para registrar errores en Log
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
                var vistaSistemasEmpresa = await _daoSistemasEmpresa.ObtenerVistaSistemasEmpresaAsync();
                if (vistaSistemasEmpresa == null || vistaSistemasEmpresa.Count == 0)
                {
                    TempData["Error"] = "No hay relaciones de sistemas a empresa disponibles.";
                    return View(new List<VistaSistemasEmpresaViewModel>());
                }
                await _bitacoraService.RegistrarBitacoraAsync("Vista Relación Sistemas a Empresa", "Ingreso a la vista de relación de sistemas a empresa");
                return View(vistaSistemasEmpresa);
            }
            catch (Exception e)
            {
                await RegistrarError("Error Vista Relación Sistemas a Empresa", e);
                return View(new List<VistaSistemasEmpresaViewModel>());
            }
        }

        // Vista para crear una nueva relación de sistema a empresa
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            // Obtenemos las listas de empresas y sistemas desde los DAOs
            var empresas = await _daoEmpresas.ObtenerEmpresasAsync();
            var sistemas = await _daoSistemas.ObtenerSistemasAsync();
            // Preparamos el modelo para la vista
            var model = new SistemasEmpresaFormViewModel
            {
                // Asignamos la lista de usuarios para el dropdown
                Empresas = empresas?.Select(u => new SelectListItem
                {
                    Value = u.IdEmpresa.ToString(),
                    Text = u.Nombre
                }).ToList() ?? new List<SelectListItem>(),
                // Asignamos la lista de roles para el dropdown
                Sistemas = sistemas?.Select(r => new SelectListItem
                {
                    Value = r.IdSistema.ToString(),
                    Text = r.Nombre
                }).ToList() ?? new List<SelectListItem>()
            };
            // Verificamos si hay empresas y sistemas disponibles para asignar
            if (model.Empresas.Count == 0 || model.Sistemas.Count == 0)
            {
                TempData["Error"] = "No hay empresas o sistemas disponibles para asignar. Por favor, asegúrese de que existan registros en las tablas correspondientes.";
                return RedirectToAction(nameof(Index));
            }
            // Enviamos a la bitácora el ingreso a la vista de relación de sistemas a empresa
            await _bitacoraService.RegistrarBitacoraAsync("Vista Relación Sistemas a Empresa", "Creación de una nueva relación de sistemas a empresa");
            // Retorna la vista con el modelo preparado
            return View(model);
        }

        // Acción para crear una nueva relación de sistema a empresa
        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(SistemasEmpresaFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Empresas = (await _daoEmpresas.ObtenerEmpresasAsync()).Select(e => new SelectListItem
                    {
                        Value = e.IdEmpresa.ToString(),
                        Text = e.Nombre
                    }).ToList();
                    model.Sistemas = (await _daoSistemas.ObtenerSistemasAsync()).Select(s => new SelectListItem
                    {
                        Value = s.IdSistema.ToString(),
                        Text = s.Nombre
                    }).ToList();
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key].Errors;
                        foreach (var error in errors)
                        {
                            // Puedes poner un breakpoint aquí o loguear el error
                            Console.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");
                        }
                    }
                    return View(model);
                }
                foreach (int sistemasId in model.FK_IdsSistema)
                {
                    // Convertimos SistemasEmpresaFormViewModel a SistemasEmpresaViewModel
                    var nuevoSistemaEmpresa = new SistemasEmpresaViewModel
                    {
                        FK_IdEmpresa = model.SistemasEmpresa.FK_IdEmpresa,
                        FK_IdSistema = sistemasId // Asignamos el ID del sistema directamente
                    };
                    await _daoSistemasEmpresa.InsertarSistemasEmpresaAsync(nuevoSistemaEmpresa);
                }
                await _bitacoraService.RegistrarBitacoraAsync("CrearRolPermisos", "Rol y permisos creados exitosamente");
                TempData["SuccessMessage"] = "Rol y permisos creados correctamente.";
                return RedirectToAction(nameof(Crear));
            }
            catch (Exception e)
            {
                await RegistrarError("Crear ", e);
                ModelState.AddModelError(string.Empty, "Error al crear la relación: " + e.Message);
                return View(model);
            }
        }
    }
}