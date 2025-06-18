using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemasEmpresaController : Controller
    {
        public IActionResult Index(SistemasEmpresaViewModel sistemasEmpresa)
        {
            return View(sistemasEmpresa);
        }
    }
}
