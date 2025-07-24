using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [AuthorizeRole("Empleado")]
        public ActionResult Index()
        {
            return View();
        }

        // Vista principal para crear solicitudes
        // GET: SolicitudesController/Crear
        [AuthorizeRole("Empleado")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            // Intenta acceder a la vista de creación de sistema y registrar la acción en la bitácora
            try
            {

                await _bitacoraService.RegistrarBitacoraAsync("Vista Crear Sistema", "Acceso a la vista de creación de sistema");
                return View(new SistemaViewModel());
            }
            // Captura cualquier excepción que ocurra durante el proceso
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de sistema", ex);
                return RedirectToAction("Index");
            }
        }

        // POST: SolicitudesController/Crear
        [AuthorizeRole("Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(SolicitudViewModel solicitud)
        {
            try
            {
                
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                // Manejo de errores, posiblemente registrar el error y mostrar un mensaje al usuario
                ModelState.AddModelError("Error al intentar crear una solicitud", "Ocurrió un error al crear la solicitud.");
                return View();
            }

        }
        // Vista principal para autorizar solicitudes
        // GET: SolicitudesController/Solicitudes
        [AuthorizeRole("Autorizador", "TeamLider", "SubTeamLider")]
        public ActionResult Solicitudes()
        {
            return View();
        }


        // Vista principal para autorizar solicitudes
        // GET: SolicitudesController/Solicitudes/Detalle
        [AuthorizeRole("Autorizador", "TeamLider", "SubTeamLider")]
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

                solicitud.Encabezado.NombreEmpleado = HttpContext.Session.GetString("NombreEmpleado") ?? "Desconocido";

                ViewBag.Empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(solicitud.Encabezado.IdEmpleado);

                return View(solicitud);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar la solicitud: " + ex.Message;
                return RedirectToAction("Solicitudes");//eror coregido
            }
        }
    }
}
