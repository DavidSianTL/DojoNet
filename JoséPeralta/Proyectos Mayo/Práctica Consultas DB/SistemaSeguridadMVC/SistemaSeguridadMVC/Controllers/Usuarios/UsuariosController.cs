using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaSeguridadMVC.Services;

namespace SistemaSeguridadMVC.Controllers.Usuarios
{
    public class UsuariosController : Controller
    {

        // GET: UsuariosController
        public ActionResult Index()
        {
            return View();
        }

        // Creamos una consulta para traer todos los usuarios
        public async Task<IActionResult> ObtenerTodosLosUsuarios()
        {
            return View();
        }

    }
}
