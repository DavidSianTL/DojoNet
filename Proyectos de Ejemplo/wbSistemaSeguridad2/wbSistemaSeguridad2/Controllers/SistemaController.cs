
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using wbSistemaSeguridad2.Data;
using wbSistemaSeguridad2.Models;
using wbSistemaSeguridad2.Services;

using Microsoft.AspNetCore.Mvc;


namespace wbSistemaSeguridad2.Controllers
{
    public class SistemaController : Controller
    {
        private readonly daoSistema _db;
        private string connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";


        public SistemaController()
        {
            //_connectionString = connectionString;
            _db = new daoSistema(connectionString);
        }
        public IActionResult Index()
        {
            try
            {
                var listaSistemas = _db.ObtenerSistemas();
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
        public ActionResult Crear(Sistema sistema)
        {
            if (ModelState.IsValid)
            {
                _db.CrearProc(sistema);
                return RedirectToAction("Index");
            }
            return View(sistema);
        }



        public ActionResult Editar(int id)
        {
            var sistema = _db.ObtenerSistemaPorId(id);
            return View(sistema);
        }

        [HttpPost]
        public ActionResult Editar(Sistema sistema)
        {
            if (ModelState.IsValid)
            {
                _db.ActualizarProc(sistema);
                return RedirectToAction("Index");
            }
            return View(sistema);
        }



        public ActionResult Eliminar(int id)
        {
            var sistema = _db.ObtenerSistemaPorId(id);
            return View(sistema);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult ConfirmarEliminar(int id)
        {
            _db.EliminarProc(id);
            return RedirectToAction("Index");
        }


    }
}
