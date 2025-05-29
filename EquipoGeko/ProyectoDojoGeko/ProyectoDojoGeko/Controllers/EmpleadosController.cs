using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    public class EmpleadosController : Controller
    {
        // Instancia del DAO para acceder a la base de datos
        private readonly daoEmpleadoWSAsync _dao;

        // Constructor que inyecta la configuración para obtener la cadena de conexión
        public EmpleadosController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _dao = new daoEmpleadoWSAsync(connectionString);
        }

        // Acción que muestra la lista de empleados
        public async Task<IActionResult> Index()
        {
            var empleados = await _dao.ObtenerEmpleadoAsync();
            return View(empleados);
        }

        // Acción que muestra el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción que procesa el formulario de creación (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        //CAMBIAR NOMBRE A LA VISTA DE "CREAR" POR EL NOMBRE VERDADERO.
        public async Task<IActionResult> CREAR(EmpleadoViewModel empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.InsertarEmpleadoAsync(empleado);
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // Acción que muestra los detalles de un empleado
        //CAMBIAR NOMBRE A LA VISTA DE "LISTAR" POR EL NOMBRE VERDADERO.

        public async Task<IActionResult> LISTAR(int id)
        {
            var empleado = await _dao.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // Acción que muestra el formulario de edición
                //CAMBIAR NOMBRE A LA VISTA DE "Create" POR EL NOMBRE VERDADERO.

        public async Task<IActionResult> EDITAR(int id)
        {
            var empleado = await _dao.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // Acción que procesa el formulario de edición (POST)
        //CAMBIAR NOMBRE A LA VISTA DE "EDITAR" POR EL NOMBRE VERDADERO.
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

        // Acción que muestra el formulario de confirmación para eliminar
        //CAMBIAR NOMBRE A LA VISTA DE "ELIMINAR" POR EL NOMBRE VERDADERO.
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var empleado = await _dao.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        // Acción que elimina al empleado (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarEmpleadoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
