using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// Usamos Mvc.Rendering para enviar tanto el Id del estado como el nombre del estado al ViewBag
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{

    [AuthorizeSession]
    public class SolicitudesController : Controller
    {
        #region INYECCIÓN DE DEPENDENCIAS

        // Instanciamos el daoEmpleado
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoSolicitudesAsync _daoSolicitud;
        private readonly ILoggingService _loggingService;
        private readonly IBitacoraService _bitacoraService;
        private readonly IEstadoService _estadoService;

        public SolicitudesController(daoEmpleadoWSAsync daoEmpleado, daoSolicitudesAsync daoSolicitud, ILoggingService loggingService, IBitacoraService bitacoraService, IEstadoService estadoService)
        {
            _daoEmpleado = daoEmpleado;
            _daoSolicitud = daoSolicitud;
            _loggingService = loggingService;
            _bitacoraService = bitacoraService;
            _estadoService = estadoService;
        }

        #endregion

        // Método para registrar errores en el log
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

        // Vista principal para ver todas las solicitudes
        // GET: SolicitudesController

        [AuthorizeRole("Empleado", "SuperAdministrador")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Extraemos los datos del empleado desde la sesión
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(HttpContext.Session.GetInt32("IdUsuario") ?? 0);

                if (empleado == null)
                {
                    await RegistrarError("acceder a la vista de creación de sistema", new Exception("Empleado no encontrado en la sesión."));
                    return RedirectToAction("Index");
                }

                // Obtiene todas las solicitudes y sus detalles
                var solicitudes = await _daoSolicitud.ObtenerSolicitudesPorEmpleadoAsync(empleado.IdEmpleado);

                // Obtiene todos los estados activos para el dropdown
                var estados = await _estadoService.ObtenerEstadosActivosAsync();

                // Le decimos que es de tipo double para que pueda manejar decimales
                ViewBag.DiasDisponibles = (double)(empleado.DiasVacacionesAcumulados);

                // Mandamos los estados al ViewBag para usarlos en la vista
                ViewBag.Estados = estados.Select(e => new SelectListItem
                {
                    Value = e.IdEstado.ToString(), // <-- Así lo espera el SelectListItem
                    Text = e.Estado
                }).ToList();

                // Registramos la acción en la bitácora
                await _bitacoraService.RegistrarBitacoraAsync("Vista Solicitudes", "Acceso a la vista de solicitudes exitosamente");

                return View(solicitudes);

            }
            catch (Exception ex)
            {
                // Registra el error y redirige a la página de inicio
                await RegistrarError("acceder a la vista de solicitudes", ex);
                return RedirectToAction("Index", "Home");
            }
            
        }

        //Solicitudes RRHH
        [AuthorizeRole("SuperAdministrador", "Autorizador", "TeamLider", "SubTeamLider")]
        public async Task<ActionResult> RecursosHumanos()
        {
            var solicitudes = await _daoSolicitud.ObtenerSolicitudEncabezadoSinFiltro();
            return View(solicitudes);
        }

        [AuthorizeRole("SuperAdministrador", "Autorizador", "TeamLider", "SubTeamLider")]
        public ActionResult DetalleRH()
        {
            return View();
        }

        // Vista principal para crear solicitudes
        // GET: SolicitudesController/Crear
        // Vista principal para crear solicitudes (formulario)
        [AuthorizeRole("Empleado", "SuperAdministrador")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            try
            {
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (!idUsuario.HasValue || idUsuario.Value == 0)
                {
                    await RegistrarError("Crear Solicitud", new Exception("El ID del usuario no se encontró en la sesión."));
                    return RedirectToAction("Index", "Home");
                }

                // 1. Obtener el objeto empleado completo, como en la vista Index.
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(idUsuario.Value);
                if (empleado == null)
                {
                    await RegistrarError("Crear Solicitud", new Exception("No se pudo encontrar el empleado."));
                    return RedirectToAction("Index", "Home");
                }

                // Unificar lógica: Preparar ViewBag y Sesión como en la vista Index.
                var nombreCompleto = $"{empleado.NombresEmpleado} {empleado.ApellidosEmpleado}";
                HttpContext.Session.SetString("NombreCompletoEmpleado", nombreCompleto);
                ViewBag.DiasDisponibles = (double)empleado.DiasVacacionesAcumulados;

                await _bitacoraService.RegistrarBitacoraAsync("Vista Crear Solicitud", "Acceso a la vista de creación de solicitud.");

                return View();
            }
            catch (Exception ex)
            {
                await RegistrarError("Acceder a la vista de creación de solicitud", ex);
                return RedirectToAction("Index", "Home");
            }
        }

        [AuthorizeRole("Empleado", "SuperAdministrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(SolicitudViewModel solicitud)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Debug: Log error de validación
                    await RegistrarError("Crear Solicitud", new Exception("Modelo no válido al crear solicitud de vacaciones."));
                    return View(solicitud);
                }

                solicitud.Encabezado.NombreEmpleado = HttpContext.Session.GetString("NombreCompletoEmpleado") ?? "Desconocido";
                solicitud.Encabezado.FechaIngresoSolicitud = DateTime.UtcNow;
                solicitud.Encabezado.Estado = 1;
                solicitud.Encabezado.IdEmpleado = HttpContext.Session.GetInt32("IdEmpleado") ?? 0;

                await _daoSolicitud.InsertarSolicitudAsync(solicitud);

                return RedirectToAction(nameof(Crear));
            }
            catch (Exception ex)
            {
                // Debug: Log error detallado
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Debug - Error Excepción",
                    Descripcion = $"Error: {ex.Message}, StackTrace: {ex.StackTrace}",
                    Estado = false
                });

                await RegistrarError("crear solicitud de vacaciones", ex);
                // PRUEBA: Mostrar el error detallado para depuración
                ModelState.AddModelError("", $"Ocurrió un error al crear la solicitud. Detalle: {ex.Message}");
                return View(solicitud);
            }
        }

        // Vista principal para autorizar solicitudes
        // GET: SolicitudesController/Solicitudes
        [AuthorizeRole("Autorizador", "TeamLider", "SubTeamLider", "SuperAdministrador")]
        public async Task<ActionResult> Autorizar()
        {
            await _bitacoraService.RegistrarBitacoraAsync("Vista Autorizar", "Acceso a la vista Autorizar exitosamente");
            var solicitudes = new List<SolicitudEncabezadoViewModel>();

            try
            {
                var rolUsuario = HttpContext.Session.GetString("Rol");
                if (rolUsuario == null) return RedirectToAction("Index", "Login"); // Si el usuario no está logeado se redirige al login

                var idAutorizador = HttpContext.Session.GetInt32("IdUsuario");
                if (idAutorizador == null) return RedirectToAction("Index", "Login"); // Si el usuario no tiene Id se redirige al login

                if (rolUsuario == "TeamLider" || rolUsuario == "SubTeamLider")
                {
                    solicitudes = await _daoSolicitud.ObtenerSolicitudEncabezadoAsync(idAutorizador); // Se obtienen las solicitudes pendientes de su equipo
                }
                else if (rolUsuario == "Autorizador" || rolUsuario == "SuperAdministrador")
                {
                    Console.WriteLine("ROL: " + rolUsuario);
                    solicitudes = await _daoSolicitud.ObtenerSolicitudEncabezadoAsync(); // Se obtienen las solicitudes pendientes sin filtrar
                }

                await _bitacoraService.RegistrarBitacoraAsync("Vista Autorizar", "Obtener lista detalles de solicitudes");
                return View(solicitudes);

            }
            catch (Exception ex)
            {
                // Log the error and redirect to the Index action (hace falta DI)***
                await RegistrarError("autorizar solicitudes", ex);
                return RedirectToAction("Index", "Login");

            }
        }

        /*------------Detalle vista ------------*/

        // Vista principal para autorizar solicitudes
        // GET: SolicitudesController/Solicitudes/Detalle
        [AuthorizeRole("Autorizador", "TeamLider", "SubTeamLider", "SuperAdministrador")]
        public async Task<ActionResult> Detalle(int id)
        {
            try
            {
                var solicitud = await _daoSolicitud.ObtenerDetalleSolicitudAsync(id);

                if (solicitud == null)
                {
                    TempData["ErrorMessage"] = "La solicitud no fue encontrada.";
                    return RedirectToAction("Solicitudes");
                }

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (!idUsuario.HasValue || idUsuario.Value == 0)
                {
                    await RegistrarError("Crear Solicitud", new Exception("El ID del usuario no se encontró en la sesión."));
                    return RedirectToAction("Index", "Home");
                }

                // 1. Obtener el objeto empleado completo, como en la vista Index.
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(idUsuario.Value);
                if (empleado == null)
                {
                    await RegistrarError("Crear Solicitud", new Exception("No se pudo encontrar el empleado."));
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Empleado = empleado;

                return View(solicitud);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar la solicitud: " + ex.Message;
                return RedirectToAction("Solicitudes");//eror coregido
            }
        }



        /*----------ErickDev-------*/
        /*Este método carga los datos de una solicitud específica */
        [AuthorizeRole("Autorizador", "TeamLider", "SubTeamLider", "SuperAdministrador")]
        public async Task<ActionResult> DetalleRH(int id)
        {
            try
            {
                var solicitud = await _daoSolicitud.ObtenerDetalleSolicitudAsync(id);

                if (solicitud == null)
                {
                    TempData["ErrorMessage"] = "La solicitud no fue encontrada.";
                    return RedirectToAction("Solicitudes");
                }

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (!idUsuario.HasValue || idUsuario.Value == 0)
                {
                    await RegistrarError("DetalleRH", new Exception("ID de usuario no encontrado en sesión."));
                    return RedirectToAction("Index", "Home");
                }

                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(idUsuario.Value);
                if (empleado == null)
                {
                    await RegistrarError("DetalleRH", new Exception("Empleado no encontrado."));
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Empleado = empleado;
                return View("DetalleRH", solicitud);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar la solicitud: " + ex.Message;
                return RedirectToAction("Solicitudes");
            }
        }
        /*-----End ErickDev---------*/

    }
}
