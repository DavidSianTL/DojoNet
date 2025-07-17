using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class SolicitudesController : Controller
    {
        // GET: SolicitudesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SolicitudesController/Crear
        public ActionResult Crear()
        {
            return View();
        }

    }
}
