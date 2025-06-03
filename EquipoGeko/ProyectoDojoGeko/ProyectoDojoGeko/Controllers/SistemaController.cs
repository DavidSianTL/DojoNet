using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemaController : Controller
    {
        private readonly daoSistemaWSAsync _daoSistema;

        public SistemaController()
        {
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _daoSistema = new daoSistemaWSAsync(_connectionString);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeRole("SuperAdmin", "Admin")]
        [HttpGet]
        public IActionResult AgregarSistema()
        {
            return View("Agregar", "Sistema");
        }

        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public IActionResult EditarSistema()
        {
            return View("Editar", "Sistema");
        }

        [HttpPost]
        public IActionResult EliminarSistema(int Id)
        {
            // Lógica para eliminar sistema
            return RedirectToAction("Index");
        }
    }
}
