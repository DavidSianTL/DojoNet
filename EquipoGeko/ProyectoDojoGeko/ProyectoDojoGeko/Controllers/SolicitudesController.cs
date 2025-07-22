using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SolicitudesController : Controller
    {

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
                // Redirigir a la lista de solicitudes o a una vista de éxito
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
        public ActionResult Detalle(int id)
        {
            // Aquí iría la lógica para obtener los detalles de la solicitud por ID
            // Por ejemplo, consultar en la base de datos y pasar el modelo a la vista
            return View();
        }



    }
}
