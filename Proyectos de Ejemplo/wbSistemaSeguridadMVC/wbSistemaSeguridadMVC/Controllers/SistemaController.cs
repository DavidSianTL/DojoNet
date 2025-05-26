using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using wbSistemaSeguridadMVC.Data;
using wbSistemaSeguridadMVC.Models;
using wbSistemaSeguridadMVC.Services;

namespace wbSistemaSeguridadMVC.Controllers
{
    public class SistemaController : Controller
    {
        private readonly daoSistemasAsync _db;
        //private readonly string _connectionString;
       

        private string connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";


        public SistemaController() {
             //_connectionString = connectionString;
            _db = new daoSistemasAsync(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var listaSistemas = await _db.ObtenerSistemasAsync();
                return View(listaSistemas);

            }
            catch (Exception ex)
            {
                return Content("Error al obtener los Sistemas: " + ex.ToString());
            }

        }


        public async Task<ActionResult> TestConexion()
        {
            try
            {
                //string connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";

                var db = new daoSistemasAsync(connectionString);
                var sistemas = await db.ObtenerSistemasAsync();
                return Content($"Conexion exitosa. Se encontraron {sistemas.Count} sistemas");
            }
            catch (Exception ex)
            {
                return Content("Error al conectar: " + ex.ToString());
            }

        }

        public ActionResult Crear() {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Crear(Sistema sistema)
        {
            if (ModelState.IsValid) {
                await _db.InsertarSistemaAsync(sistema);
                return RedirectToAction("Index");

            }
            return View(sistema);

        }

        public async Task<ActionResult> Editar(int Id)
        {
            var sistema = await _db.ObtenerSistemaPorIdAsync(Id);
            return View(sistema);

        }

        [HttpPost]
        public async Task<ActionResult> Editar(Sistema sistema) 
        {
            if (ModelState.IsValid)
            {
                await _db.ActualizarSistemaAsync(sistema);
                return RedirectToAction("Index");
            }
            return View(sistema);
        
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var sistema = await _db.ObtenerSistemaPorIdAsync(id);
            return View(sistema);   
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            await _db.EliminarSistemaAsync(id);
            return RedirectToAction("Index");   
        
        }

    }
}
