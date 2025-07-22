using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
//ErickDev: Using adicionales necesarios
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
/*End ErickDev*/

namespace ProyectoDojoGeko.Controllers
{

    [AuthorizeSession]
    public class SolicitudesController : Controller
    {
        /*Solo para que funcione hice el constructor*/
        private readonly daoSolicitudesAsync _daoSolicitudes;
        public SolicitudesController(IConfiguration configuration)
        {
            _daoSolicitudes = new daoSolicitudesAsync(configuration.GetConnectionString("DefaultConnection"));
        }
        /*-----*/

        // Vista principal para ver todas las solicitudes
        // GET: SolicitudesController
        [AuthorizeRole("Empleado")]
        public ActionResult Index()
        {
            return View();
        }

        // Vista principal para crear solicitudes
        // GET: SolicitudesController/Crear
        [AuthorizeRole("Empleado")]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: SolicitudesController/Crear
        [HttpPost]
        [AuthorizeRole("Empleado")]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(IFormCollection collection)
        {
            try
            {
                // Aquí iría la lógica para crear una solicitud
                // Por ejemplo, guardar en la base de datos
                // Redirigir a la lista de solicitudes o a una vista de éxito
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Manejo de errores, posiblemente registrar el error y mostrar un mensaje al usuario
                ModelState.AddModelError("", "Ocurrió un error al crear la solicitud.");
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


        //EirckDev:
        /* ------ */

        public async Task<ActionResult> Detalle(int id)
        {
            try
            {
                var solicitud = await _daoSolicitudes.ObtenerDetalleSolicitudAsync(id);

                if (solicitud == null)
                {
                    TempData["ErrorMessage"] = "La solicitud no fue encontrada.";
                    return RedirectToAction(nameof(Solicitudes));
                }

                return View(solicitud);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar la solicitud: " + ex.Message;
                return RedirectToAction(nameof(Solicitudes));
            }
        }
        /*------*/
        /*End ErickDev*/
    }
}
