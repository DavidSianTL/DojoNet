using Microsoft.AspNetCore.Mvc;

namespace Manejo_de_Errores.Controllers
{
    public class EstadisticasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
