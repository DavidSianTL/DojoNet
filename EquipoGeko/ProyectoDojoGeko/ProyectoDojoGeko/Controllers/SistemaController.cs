using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemaController : Controller
    {
        private readonly daoSistemaWSAsync _daoSistema;

        public SistemaController()
        {
            string _connectionString = "Server=DESKTOP-GOBNNQ6\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _daoSistema = new daoSistemaWSAsync(_connectionString);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var sistemas = await _daoSistema.ObtenerSistemasAsync();
                ;

                if (sistemas != null && sistemas.Any())
                    return View(sistemas);

                return View(new List<SistemaViewModel>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener sistemas: {ex.Message}");
                ViewBag.Error = "Error al conectar con la base de datos.";
                return View(new List<SistemaViewModel>());
            }
        }

        [AuthorizeRole("SuperAdmin", "Admin")]
        [HttpGet]
        public IActionResult AgregarSistema()
        {
            return View("Agregar"); 
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
