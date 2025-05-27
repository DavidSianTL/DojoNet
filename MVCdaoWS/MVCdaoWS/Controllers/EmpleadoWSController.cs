using Microsoft.AspNetCore.Mvc;
using MVCdaoWS.Data;
using MVCdaoWS.Models;
using System;
using System.Threading.Tasks;

namespace MVCdaoWS.Controllers
{
    public class EmpleadoWSController : Controller
    {
        private readonly daoEmpleadoAsyncWS _dao;

        public EmpleadoWSController()
        {
            var connectionString = "Server=CARLOSC;Database=EmpresaDB;Integrated Security=True;TrustServerCertificate=True;";
            _dao = new daoEmpleadoAsyncWS(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _dao.ObtenerTodosAsync();
            return View(lista);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(EmpleadoWS empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.CrearAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var empleado = await _dao.ObtenerPorIdAsync(id);
            return View(empleado);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(EmpleadoWS empleado)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarAsync(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var empleado = await _dao.ObtenerPorIdAsync(id);
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            await _dao.EliminarAsync(id);
            return RedirectToAction("Index");
        }
    }
}
