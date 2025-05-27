using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using wbEjercicioEmpleadosMVC.Models;
using wbEjercicioEmpleadosMVC.Data;

namespace wbEjercicioEmpleadosMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly daoEmpleadoAsync _db;

        private string connectionString = "Server=DESKTOP-GOBNNQ6\\SQLEXPRESS;Database=EmpresaDB;Integrated Security=True;TrustServerCertificate=True;";

        public EmpleadoController()
        {
            //_connectionString = connectionString;
            _db = new daoEmpleadoAsync(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var listaEmpleados = await _db.ObtenerEmpleadoAsync();
                return View(listaEmpleados);

            }
            catch (Exception ex)
            {
                return Content("Error al obtener la lista de Empleados: " + ex.ToString());
            }
        }

        public async Task<ActionResult> TestConexion()
        {
            try
            {
                var db = new daoEmpleadoAsync(connectionString);
                var empleados = await db.ObtenerEmpleadoAsync();
                return Content($"Conexion exitosa. Se encontraron {empleados.Count} empleados");
            }
            catch (Exception ex)
            {
                return Content("Error al conectar con la BD: " + ex.ToString());
            }

        }

        public ActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(EmpleadoModel empleado)
        {
            if (ModelState.IsValid)
            {
                await _db.InsertarEmpleadoAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public async Task<ActionResult> Editar(int Id)
        {
            var empleado = await _db.ObtenerEmpleadoPorIdAsync(Id);
            return View(empleado);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(EmpleadoModel empleado)
        {
            if (ModelState.IsValid)
            {
                await _db.ActualizarEmpleadoAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var empleado = await _db.ObtenerEmpleadoPorIdAsync(id);
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            await _db.EliminarEmpleadoAsync(id);
            return RedirectToAction("Index");
        }
    }
}
