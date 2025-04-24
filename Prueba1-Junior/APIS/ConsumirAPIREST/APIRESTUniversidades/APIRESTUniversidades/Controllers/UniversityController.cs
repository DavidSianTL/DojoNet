using Microsoft.AspNetCore.Mvc;

namespace APIRESTUniversidades.Controllers
{
    public class UniversityController : Controller
    {
        public IActionResult UniversidadPorPais()
        {
            ViewBag.Message = "Marianito Galvez";
            return View();
        }
    }
}
