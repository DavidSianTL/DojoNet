
using System.Threading.Tasks;
using System.Configuration;
using wbSistemaSeguridadMVC.Models;
using Microsoft.AspNetCore.Mvc;
using wbSistemaSeguridadMVC.Services;
using wbSistemaSeguridadMVC.Data;


namespace wbSistemaSeguridadMVC.Controllers
{
    public class SistemaWSController : Controller
    {
        private readonly daoSistemaAsyncWS _dao;

        public SistemaWSController()
        {
            var connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";
            _dao = new daoSistemaAsyncWS(connectionString);
        }


        public async Task<IActionResult> Index()
        {
           try
            {
                var listaSistemas = await _dao.ObtenerTodosAsync();
                return View(listaSistemas);
            }
            catch (Exception ex)
            {
                return Content("Error al obtener los Sistemas: " + ex.Message);

            }
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(SistemaWS sistema)
        {
            if (ModelState.IsValid)
            {
                await _dao.CrearAsync(sistema);
                return RedirectToAction("Index");
            }
            return View(sistema);
        }

        public async Task<ActionResult> Editar(int id)
        {
            var sistema = await _dao.ObtenerPorIdAsync(id);
            return View(sistema);
        }

        [HttpPost]
        public async Task<ActionResult> Editar(SistemaWS sistema)
        {
            if (ModelState.IsValid)
            {
                await _dao.ActualizarAsync(sistema);
                return RedirectToAction("Index");
            }
            return View(sistema);



        }



        public async Task<ActionResult> Eliminar(int id)
        {
            var sistema = await _dao.ObtenerPorIdAsync(id);
            return View(sistema);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            await _dao.EliminarAsync(id);
            return RedirectToAction("Index");
        }


    }
}
