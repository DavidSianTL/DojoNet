using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;
using ProyectoDojoGeko.Dtos.Empleados.Requests;
using ProyectoDojoGeko.Dtos.Empleados.Responses;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpleadosController : Controller
    {
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly ILoggingService _loggingService;
        private readonly IEstadoService _estadoService;

        public EmpleadosController(
            daoEmpleadoWSAsync daoEmpleado,
            daoUsuarioWSAsync daoUsuario,
            daoRolesWSAsync daoRoles,
            daoSistemaWSAsync daoSistema,
            daoBitacoraWSAsync daoBitacora,
            ILoggingService loggingService,
            IEstadoService estadoService)
        {
            _daoEmpleado = daoEmpleado;
            _daoUsuario = daoUsuario;
            _daoRoles = daoRoles;
            _daoSistema = daoSistema;
            _daoBitacora = daoBitacora;
            _loggingService = loggingService;
            _estadoService = estadoService;
        }

        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
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
                // Extraemos los empleados
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                // Pasamos los empleados al modelo de respuesta como una lista
                var empleadosResponse = empleados?.Select(e => new EmpleadoResponse
                {
                    IdEmpleado = e.IdEmpleado,
                    NombreEmpleado = e.NombreEmpleado,
                    ApellidoEmpleado = e.ApellidoEmpleado,
                    DPI = e.DPI,
                    CorreoInstitucional = e.CorreoInstitucional,
                    FechaIngreso = e.FechaIngreso,
                    Genero = e.Genero,
                    Estado = e.Estado
                }).ToList() ?? new List<EmpleadoResponse>();

                // Obtenemos los estados usando el servicio
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();

                // Agregamos la acción a la bitácora
                await RegistrarBitacora("Vista Empleados", "Acceso a lista de empleados");

                // Devolvemos la respuesta
                return View(empleadosResponse);
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a vista de empleados", ex);
                return View(new List<EmpleadoResponse>());
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                // Obtenemos los estados usando el servicio
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();

                // Agregamos la acción a la bitácora
                await RegistrarBitacora("Vista Empleados", "Acceso a vista creación de empleados");

                // Devolvemos la respuesta
                return View();
            }
            catch (Exception ex)
            {
                await RegistrarError("acceso a vista creación de empleados", ex);
                return View(new List<CrearEmpleadoRequest>());
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(CrearEmpleadoRequest empleado)
        {
            try
            {
                // Verifica si el modelo del empleado es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    // Devolvemos el error a Logs
                    await RegistrarError("crear empleado - datos inválidos", new Exception("Validación de modelo fallida"));
                    // Obtenemos los estados usando el servicio
                    ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();
                    //
                    return View(empleado);
                }

                // Mapeamos el modelo CrearEmpleadoRequest al modelo EmpleadoViewModel
                // Para poder insertar el nuevo empleado en la base de datos
                var empleadoViewModel = new EmpleadoViewModel
                {
                    DPI = empleado.DPI,
                    NombreEmpleado = empleado.NombreEmpleado,
                    ApellidoEmpleado = empleado.ApellidoEmpleado,
                    CorreoInstitucional = empleado.CorreoInstitucional,
                    Telefono = empleado.Telefono,
                    CorreoPersonal = empleado.CorreoPersonal,
                    Salario = empleado.Salario,
                    FechaIngreso = empleado.FechaIngreso,
                    FechaNacimiento = empleado.FechaNacimiento,
                    Estado = empleado.Estado
                };

                // Inserta el nuevo empleado en la base de datos de forma asíncrona
                await _daoEmpleado.InsertarEmpleadoAsync(empleadoViewModel);

                // Registra la acción de creación del empleado en la bitácora
                await RegistrarBitacora("Crear Empleado", $"Empleado creado: {empleado.NombreEmpleado}");

                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Empleado creado correctamente";

                // Redirige al usuario a la lista de empleados después de la creación exitosa
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("Error al crear empleado", ex);
                return RedirectToAction("Crear");
            }
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                // Verifica si el ID del empleado es válido
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);

                // Si el empleado no se encuentra, registra un error y devuelve NotFound
                if (empleado == null)
                {
                    await RegistrarError("editar empleado - no encontrado", new Exception($"Empleado con ID {id} no encontrado"));
                    return NotFound();
                }

                // Mapeamos al modelo ActualizarEmpleadoRequest
                var empleadoRequest = new ActualizarEmpleadoRequest
                {
                    IdEmpleado = empleado.IdEmpleado,
                    NombreEmpleado = empleado.NombreEmpleado,
                    ApellidoEmpleado = empleado.ApellidoEmpleado,
                    CorreoInstitucional = empleado.CorreoInstitucional,
                    CorreoPersonal = empleado.CorreoPersonal,
                    Telefono = empleado.Telefono,
                    NIT = empleado.NIT,
                    Genero = empleado.Genero,
                    Salario = empleado.Salario,
                    Estado = empleado.Estado
                };

                // Obtenemos los estados usando el servicio
                ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();

                // Registra la acción de acceso a la vista de edición del empleado
                await RegistrarBitacora("Vista Editar Empleados", $"Acceso a edición de empleado: {empleado.NombreEmpleado} (ID: {id})");

                // Devuelve la vista de edición con el modelo correcto
                return View(empleadoRequest);
            }
            catch (Exception ex)
            {
                await RegistrarError("acceso a vista edición de empleado", ex);
                return RedirectToAction("Index");
            }
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [HttpPost]
        public async Task<IActionResult> Editar(ActualizarEmpleadoRequest empleado)
        {
            try
            {
                // Verifica si el modelo del empleado es válido antes de proceder
                if (!ModelState.IsValid)
                {
                    // Devolvemos el error a Logs
                    await RegistrarError("editar empleado - datos inválidos", new Exception("Validación de modelo fallida"));
                    // Obtenemos los estados usando el servicio
                    ViewBag.Estados = await _estadoService.ObtenerEstadosActivosAsync();
                    //
                    return View(empleado);
                }

                // Mapeamos el modelo CrearEmpleadoRequest al modelo EmpleadoViewModel
                // Para poder insertar el nuevo empleado en la base de datos
                var empleadoViewModel = new EmpleadoViewModel
                {
                    IdEmpleado = empleado.IdEmpleado,
                    NIT = empleado.NIT,
                    NombreEmpleado = empleado.NombreEmpleado,
                    ApellidoEmpleado = empleado.ApellidoEmpleado,
                    Genero = empleado.Genero,
                    CorreoInstitucional = empleado.CorreoInstitucional,
                    Telefono = empleado.Telefono,
                    CorreoPersonal = empleado.CorreoPersonal,
                    Salario = empleado.Salario,
                    Estado = empleado.Estado
                };

                // Inserta el nuevo empleado en la base de datos de forma asíncrona
                await _daoEmpleado.ActualizarEmpleadoAsync(empleadoViewModel);

                // Registra la acción de creación del empleado en la bitácora
                await RegistrarBitacora("Editar Empleado", $"Empleado editado: {empleado.NombreEmpleado}");

                // Muestra un mensaje de éxito al usuario
                TempData["SuccessMessage"] = "Empleado editado correctamente";

                // Redirige al usuario a la lista de empleados después de la edición exitosa
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                await RegistrarError("acceso a vista edición de empleado", ex);
                return RedirectToAction("Editar");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> ELIMINAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                if (empleado == null)
                {
                    await RegistrarError("eliminar empleado", new Exception($"Empleado {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Vista Eliminar Empleado", $"ID: {id}");
                return View(empleado);
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar empleado", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> ELIMINAR(int id, IFormCollection collection)
        {
            try
            {
                await _daoEmpleado.EliminarEmpleadoAsync(id);
                await RegistrarBitacora("Empleado Eliminado", $"ID: {id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar empleado", ex);
                return RedirectToAction("ELIMINAR", new { id });
            }
        }
    }
}

