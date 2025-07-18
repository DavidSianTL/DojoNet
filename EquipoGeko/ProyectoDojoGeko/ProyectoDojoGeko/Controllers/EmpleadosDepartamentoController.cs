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
    public class EmpleadosDepartamentoController : Controller
    {
        private readonly daoEmpleadosDepartamentoWSAsync _daoEmpleadosDepartamento;
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoDepartamentoWSAsync _daoDepartamento;
        private readonly IBitacoraService _bitacoraService;
        private readonly daoLogWSAsync _daoLog;
        private readonly ILoggingService _loggingService;

        public EmpleadosDepartamentoController(
            daoEmpleadosDepartamentoWSAsync daoEmpleadosDepartamento,
            daoEmpleadoWSAsync daoEmpleado,
            daoDepartamentoWSAsync daoDepartamento,
            IBitacoraService bitacoraService,
            daoLogWSAsync daoLog,
            ILoggingService loggingService,
            EmailService emailService)
        {
            _daoEmpleadosDepartamento = daoEmpleadosDepartamento;
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
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
                // Preparar el modelo para la vista
                var model = new EmpleadosDepartamentoFormViewModel
                {
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
                return View(new EmpleadosDepartamentoFormViewModel());
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(EmpleadosDepartamentoFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Recorrer todas las combinaciones de empleados y departamentos seleccionados
                foreach (var idEmpleado in model.EmpleadosDepartamento.FK_IdsEmpleado)
                {
                    foreach (var idDepartamento in model.EmpleadosDepartamento.FK_IdsDepartamento)
                    {
                        await _daoEmpleadosDepartamento.InsertarEmpleadoDepartamentoAsync(new Models.Empleados.EmpleadosDepartamentoViewModel
                        {
                            FK_IdEmpleado = idEmpleado,
                            FK_IdDepartamento = idDepartamento
                        });
                    }
                }
                // Registrar bitácora la creación
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