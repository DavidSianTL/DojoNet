using Microsoft.AspNetCore.Mvc;
using ExamDaniel.bitacora;

namespace ExamDaniel.Controllers
{
    public class BitacoraController : Controller
    {
        public IActionResult Index()
        {
            // Obtiene los registros de la bitácora
            var registros = BitacoraManager.LeerEventos();
            return View(registros);
        }
    }
}
