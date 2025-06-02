using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class EmpresaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
