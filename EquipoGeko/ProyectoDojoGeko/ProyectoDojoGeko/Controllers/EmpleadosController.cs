using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
    public class EmpleadosController : Controller
    {
        // Instancia del DAO para acceder a la base de datos
        private readonly daoEmpleadoWSAsync _dao;

        // Constructor que inicializa la conexión con la base de datos
        public EmpleadosController(IConfiguration configuration)
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnection");
            // Conexión directa para pruebas rápidas
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoEmpleadoWSAsync(connectionString);
        }

        // Acción que muestra la lista de empleados
        public async Task<IActionResult> Index()
        {
            var empleados = await _dao.ObtenerEmpleadoAsync();
            return View(empleados);
        }

        // Acción que muestra el formulario de creación de empleado
        public IActionResult CREAR()
        {
            return View();
        }

        // Acción que procesa el formulario de creación (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CREAR(EmpleadoViewModel empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.InsertarEmpleadoAsync(empleado);
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        // Acción que muestra los detalles de un empleado específico
        public async Task<IActionResult> LISTAR(int id)
        {
            var empleado = await _dao.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // Acción que muestra el formulario de edición de un empleado
        public async Task<IActionResult> EDITAR(int id)
        {
            var empleado = await _dao.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // Acción que procesa el formulario de edición (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EDITAR(EmpleadoViewModel empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarEmpleadoAsync(empleado);
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        // Acción que muestra el formulario de confirmación para eliminar un empleado
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var empleado = await _dao.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // Acción que elimina el empleado (POST)
        [HttpPost, ActionName("ELIMINAR")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarEmpleadoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
