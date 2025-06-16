using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpleadosEmpresa : Controller
    {
        [AuthorizeRole("Administrador, Empleado")]
        public IActionResult Index()
        {
            return View();
        }

    }

}
