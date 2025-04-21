using Microsoft.AspNetCore.Mvc;

namespace ConsumoDeApis.Controllers
{
    public class UsuariosController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
