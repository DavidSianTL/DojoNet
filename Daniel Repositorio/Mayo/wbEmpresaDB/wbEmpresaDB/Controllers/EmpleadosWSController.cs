using Microsoft.AspNetCore.Mvc;
using wbEmpresaDB.Data;
using wbEmpresaDB.Models;
using wbEmpresaDB.Services;

namespace wbEmpresaDB.Controllers
{
    public class EmpleadosWSController : Controller
    {
        private readonly daoEmpleadosAsyncWS _dao;

        public EmpleadosWSController(daoEmpleadosAsyncWS dao)
        {
            var connectionString = "Server=LAPTOP-6R9HT6G4;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";
            _dao = dao;
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _dao.ObtenerTodosAsync();
            return View(empleados);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Empleado emp)
        {
            if (ModelState.IsValid)
            {
                await _dao.CrearAsync(emp);
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var emp = await _dao.ObtenerPorIdAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Empleado emp)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarAsync(emp);
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var emp = await _dao.ObtenerPorIdAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _dao.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var emp = await _dao.ObtenerPorIdAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }
    }
}
