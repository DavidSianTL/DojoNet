using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Empleados;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpleadosEmpresaDepartamentoController : Controller
    {
        private readonly daoEmpresaWSAsync _daoEmpresas;
        private readonly daoEmpleadosEmpresaDepartamentoWSAsync _daoEmpleadosEmpresaDepartamento;
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoDepartamentoWSAsync _daoDepartamento;
        private readonly IBitacoraService _bitacoraService;
        private readonly daoLogWSAsync _daoLog;
        private readonly ILoggingService _loggingService;

        public EmpleadosEmpresaDepartamentoController(
            daoEmpresaWSAsync daoEmpresas,
            daoEmpleadosEmpresaDepartamentoWSAsync daoEmpleadosEmpresaDepartamento,
            daoEmpleadoWSAsync daoEmpleado,
            daoDepartamentoWSAsync daoDepartamento,
            IBitacoraService bitacoraService,
            daoLogWSAsync daoLog,
            ILoggingService loggingService,
            EmailService emailService)
        {
            _daoEmpresas = daoEmpresas;
            _daoEmpleadosEmpresaDepartamento = daoEmpleadosEmpresaDepartamento;
            _daoEmpleado = daoEmpleado;
            _daoDepartamento = daoDepartamento;
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

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                // Obtener empleados y departamentos
                var empresas = await _daoEmpresas.ObtenerEmpresasAsync();
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
                // Preparar el modelo para la vista
                var model = new EmpleadosEmpresaDepartamentoFormViewModel
                {
                    Empresas = empresas.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpresa.ToString(),
                        Text = e.Nombre 
                    }).ToList(),
                    Empleados = empleados.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = e.NombresEmpleado + " " + e.ApellidosEmpleado
                    }).ToList(),
                    Departamentos = departamentos.Select(d => new SelectListItem
                    {
                        Value = d.IdDepartamento.ToString(),
                        Text = d.Nombre
                    }).ToList()
                };

                // Registrar bitácora de acceso a la vista
                await _bitacoraService.RegistrarBitacoraAsync("Vista Crear Empleado Departamento", "Ingreso a la vista de creación de empleado departamento");
                // Retornar la vista con el modelo
                return View(model);
            }
            catch (Exception e)
            {
                // Registrar error en Log
                await RegistrarError("Crear Empleado Departamento", e);
                // Retornar vista con mensaje de error
                TempData["Error"] = "Ocurrió un error al cargar la vista de creación de empleado departamento.";
                return View(new EmpleadosEmpresaDepartamentoFormViewModel());
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(EmpleadosEmpresaDepartamentoFormViewModel model)
        {
            if (ModelState.IsValid)
            {

                Console.WriteLine($"[LOG] Empresa seleccionada: {model.EmpleadosEmpresa.FK_IdEmpresa}");
                Console.WriteLine($"[LOG] Empleados seleccionados: {string.Join(", ", model.FK_IdsEmpleado)}");

                // Validamos que empleados no venga vacio
                if(model.FK_IdsEmpleado == null || !model.FK_IdsEmpleado.Any())
                {
                    ModelState.AddModelError("", "Debe seleccionar al menos un empleado.");
                    return RedirectToAction(nameof(Crear));
                }

                // Insertar relación empresa-empleado para cada empleado
                foreach (var idEmpleado in model.FK_IdsEmpleado)
                {
                    await _daoEmpleadosEmpresaDepartamento.InsertarEmpleadoEmpresaAsync(new EmpleadosEmpresaViewModel
                    {
                        FK_IdEmpleado = idEmpleado,
                        FK_IdEmpresa = model.EmpleadosEmpresa.FK_IdEmpresa
                    });
                }

                // Insertar relación empleado-departamento para cada combinación
                foreach (var idEmpleado in model.FK_IdsEmpleado)
                {
                    foreach (var idDepartamento in model.EmpleadosDepartamento.FK_IdsDepartamento)
                    {
                        await _daoEmpleadosEmpresaDepartamento.InsertarEmpleadoDepartamentoAsync(new EmpleadosDepartamentoViewModel
                        {
                            FK_IdEmpleado = idEmpleado,
                            FK_IdDepartamento = idDepartamento
                        });
                    }
                }

                // Registrar bitácora la creación
                await _bitacoraService.RegistrarBitacoraAsync("Crear Empleado Empresa", "Se creó un nuevo empleado empresa");
                await _bitacoraService.RegistrarBitacoraAsync("Crear Empleado Departamento", "Se creó un nuevo empleado departamento");
                // Redirigir a la lista de asignaciones
                return RedirectToAction(nameof(Crear));
            }
            // Si el modelo no es válido, recargar las listas para la vista
            var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
            var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
            model.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.IdEmpleado.ToString(),
                Text = e.NombresEmpleado + " " + e.ApellidosEmpleado
            }).ToList();
            model.Departamentos = departamentos.Select(d => new SelectListItem
            {
                Value = d.IdDepartamento.ToString(),
                Text = d.Nombre
            }).ToList();
            return RedirectToAction(nameof(Crear));
        }
    }
}