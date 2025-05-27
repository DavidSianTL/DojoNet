using System.Threading.Tasks;
using System.Configuration;
using wbEjercicioEmpleadosMVC.Models;
using Microsoft.AspNetCore.Mvc;
using wbEjercicioEmpleadosMVC.Services;
using wbEjercicioEmpleadosMVC.Data;
using AspNetCoreGeneratedDocument;

namespace wbEjercicioEmpleadosMVC.Controllers
{
    public class EmpleadoControllerWS : Controller
    {
        private readonly daoEmpleadoAsyncWS _dao;

        public EmpleadoControllerWS()
        {
            var connectionString = "Server=DESKTOP-GOBNNQ6\\SQLEXPRESS;Database=EmpresaDB;Integrated Security=True;TrustServerCertificate=True;";
            _dao = new daoEmpleadoAsyncWS(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var listaEmpleados = await _dao.ObtenerTodosEmpleadosAsync();
                return View(listaEmpleados);
            }
            catch (Exception ex)
            {
                return Content("Error al obtener los Empleados: " + ex.Message);

            }
        }
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(EmpleadoModelWS empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.CrearEAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var empleado = await _dao.ObtenerEPorIdAsync(id);
            return View(empleado);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(EmpleadoModelWS empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarEAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var empleado = await _dao.ObtenerEPorIdAsync(id);
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            await _dao.EliminarEAsync(id);
            return RedirectToAction("Index");
        }
    }
}
