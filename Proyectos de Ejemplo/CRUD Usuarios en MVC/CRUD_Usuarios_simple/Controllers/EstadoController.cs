using Microsoft.AspNetCore.Mvc;

namespace CRUD_Usuarios_simple.Controllers
{
    public class EstadoController : Controller
    {
        public IActionResult CrearEstado()
        {
            return View("Estado");
        }
    }
}
