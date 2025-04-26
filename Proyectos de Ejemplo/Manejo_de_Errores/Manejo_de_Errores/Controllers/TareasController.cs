using Microsoft.AspNetCore.Mvc;

namespace Manejo_de_Errores.Controllers
{
    public class TareasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
