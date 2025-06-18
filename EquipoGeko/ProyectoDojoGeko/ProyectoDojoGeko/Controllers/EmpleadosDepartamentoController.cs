using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Empleados;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpleadosDepartamentoController : Controller
    {

        // Instanciamos el DAO
        private readonly daoEmpleadosDepartamentoWSAsync _daoEmpleadosDepartamento;

        // Instanciamos el DAO de empleados
        private readonly daoEmpleadoWSAsync _daoEmpleado;

        // Instanciamos el DAO de departamentos
        private readonly daoDepartamentoWSAsync _daoDepartamento;

        // Instanciamos el DAO de bitácora
        private readonly daoBitacoraWSAsync _daoBitacora;

        // Instanciamos el DAO de log
        private readonly daoLogWSAsync _daoLog;

        // Constructor para inicializar la cadena de conexión
        public EmpleadosDepartamentoController(EmailService emailService)
        {
            // Cadena de conexión a la DB de producción
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            // Cadena de conexión a la base de datos local
            // string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos el DAO con la cadena de conexión
            _daoEmpleadosDepartamento = new daoEmpleadosDepartamentoWSAsync(_connectionString);
            // Inicializamos el DAO de empleados con la misma cadena de conexión
            _daoEmpleado = new daoEmpleadoWSAsync(_connectionString);
            // Inicializamos el DAO de departamentos con la misma cadena de conexión
            _daoDepartamento = new daoDepartamentoWSAsync(_connectionString);
            // Inicializamos el DAO de bitácora con la misma cadena de conexión
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            // Inicializamos el DAO de log con la misma cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Método privado para registrar errores en Log
        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método privado para registrar acciones exitosas en Bitácora
        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador","Administrador","Editor")]
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
                        Text = e.NombreEmpleado + " " + e.ApellidoEmpleado
                    }).ToList(),
                    Departamentos = departamentos.Select(d => new SelectListItem
                    {
                        Value = d.IdDepartamento.ToString(),
                        Text = d.Nombre
                    }).ToList()
                };

                // Registrar bitácora de acceso a la vista
                await RegistrarBitacora("Vista Crear Empleado Departamento", "Ingreso a la vista de creación de empleado departamento");

                // Retornar la vista con el modelo
                return View(model);
            }
            catch(Exception e)
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

                // Redirigir a la lista de asignaciones
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, recargar las listas para la vista
            var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
            var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();
            model.Empleados = empleados.Select(e => new SelectListItem
            {
                Value = e.IdEmpleado.ToString(),
                Text = e.NombreEmpleado + " " + e.ApellidoEmpleado
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
