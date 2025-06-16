using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemasEmpresaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
