using Microsoft.AspNetCore.Mvc;
using MVCDao.Data;          
using MVCDao.Models;        
using System;
using System.Threading.Tasks;

namespace MVCDao.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly daoEmpleadoAsync _db;

        // Cambia esto con tu cadena de conexión real si es necesario
        private readonly string connectionString = "Server=CARLOSC;Database=EmpresaDB;Integrated Security=True;TrustServerCertificate=True;";

        public EmpleadoController()
        {
            _db = new daoEmpleadoAsync(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var empleados = await _db.ObtenerEmpleadosAsync();
                return View(empleados);
            }
            catch (Exception ex)
            {
                return Content("Error al obtener los empleados: " + ex.Message);
            }
        }

        public async Task<ActionResult> TestConexion()
        {
            try
            {
                var empleados = await _db.ObtenerEmpleadosAsync();
                return Content($"Conexión exitosa. Se encontraron {empleados.Count} empleados.");
            }
            catch (Exception ex)
            {
                return Content("Error al conectar: " + ex.Message);
            }
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                await _db.InsertarEmpleadoAsync(empleado);
                return RedirectToAction("Index");
            }

            return View(empleado);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var empleado = await _db.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(Empleado empleado)
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
            if (empleado == null)
            {
                return NotFound();
            }

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
