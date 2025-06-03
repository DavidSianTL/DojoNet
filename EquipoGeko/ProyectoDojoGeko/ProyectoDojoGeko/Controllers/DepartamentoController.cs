using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class DepartamentoController : Controller
    {
        private readonly daoDepartamentoWSAsync _daoDepartamento;

        public DepartamentoController()
        {
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _daoDepartamento = new daoDepartamentoWSAsync(_connectionString);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeRole("SuperAdmin", "Admin")]
        [HttpGet]
        public IActionResult AgregarDepartamento()
        {
            return View("Agregar", "Departamento");
        }

        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public IActionResult EditarDepartamento()
        {
            return View("Editar", "Departamento");
        }

        [HttpPost]
        public IActionResult EliminarDepartamento(int Id)
        {
            // Lógica para eliminar departamento
            return RedirectToAction("Index");
        }
    }
}


