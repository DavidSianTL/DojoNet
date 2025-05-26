using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetoCRUDEmpleados.Data;
using RetoCRUDEmpleados.Data;
using RetoCRUDEmpleados.Models;

namespace RetoCRUDEmpleados.Controllers
{
    public class EmpleadoController : Controller
    {

        // Instanciamos el DAO
        private readonly daoEmpleadoAsync _daoEmpleados;

        // Constructor para inicializar la cadena de conexión
        public EmpleadoController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBEmpresa;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoEmpleados = new daoEmpleadoAsync(_connectionString);
        }

        // GET: EmpleadoController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Llamamos al método ObtenerEmpleadosAsync para obtener la lista de empleados
                var empleados = await _daoEmpleados.ObtenerEmpleadosAsync();
                // Devolvemos la vista con la lista de empleados
                return View(empleados);
            }
            catch (Exception e)
            {   // En caso de error, devolvemos la vista con un mensaje de error
                ViewBag.Error = "Error al obtener los empleados: " + e.Message;
                return View();
            }
        }

        // GET: EmpleadoController/Crear
        [HttpGet]
        public ActionResult Crear()
        {
            // Devolvemos la vista para crear un nuevo empleado
            return View();
        }

        // POST: EmpleadoController/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(EmpleadoViewModel empleado)
        {
            try
            {
                // Si el modelo es válido, llamamos al método InsertarEmpleadoAsync
                await _daoEmpleados.InsertarEmpleadoAsync(empleado);
                // Redirigimos a la acción Index después de insertar el empleado
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {   // En caso de error, devolvemos la vista con un mensaje de error
                ViewBag.Error = "Error al insertar el empleado: " + e.Message;
                return View(empleado);
            }
        }

        // GET: EmpleadoController/Editar/1
        [HttpGet]
        public async Task<ActionResult> Editar(int Id)
        {
            var empleado = await _daoEmpleados.ObtenerEmpleadoPorIdAsync(Id);
            return View(empleado);
        }

        // POST: EmpleadoController/Editar/1
        [HttpPost]
        public async Task<ActionResult> Editar(EmpleadoViewModel empleado)
        {
            if (ModelState.IsValid)
            {
                await _daoEmpleados.ActualizarEmpleadoAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);

        }

        // POST: EmpleadoController/Eliminar/1
        [HttpPost]
        public async Task<IActionResult> Eliminar(int Id)
        {
            try
            {
                // EliminarEmpleadoAsync no devuelve un valor, por lo que no se puede asignar a una variable.
                await _daoEmpleados.EliminarEmpleadoAsync(Id);

                // Redirigimos a la acción Index después de eliminar el empleado
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                // En caso de error, devolvemos la vista con un mensaje de error
                Console.WriteLine("Error al eliminar el empleado: " + e.Message);
                return View("Index");
            }
        }


    }
}
