using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaSeguridadMVC.Data;
using SistemaSeguridadMVC.Models;

namespace SistemaSeguridadMVC.Controllers
{
    public class SistemaController : Controller
    {

        // Instanciamos el DAO
        private readonly DaoSistemasAsync _daoSistemas;

        // Constructor para inicializar la cadena de conexión
        public SistemaController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBSistemaDeSeguridad;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoSistemas = new DaoSistemasAsync(_connectionString);
        }

        // GET: SistemaController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Llamamos al método ObtenerSistemasAsync para obtener la lista de sistemas
                var sistemas = await _daoSistemas.ObtenerSistemasAsync();
                // Devolvemos la vista con la lista de sistemas
                return View(sistemas);
            }
            catch(Exception e)
            {   // En caso de error, devolvemos la vista con un mensaje de error
                ViewBag.Error = "Error al obtener los sistemas: " + e.Message;
                return View();
            }
        }

        // GET: SistemaController/Crear
        [HttpGet]
        public ActionResult Crear()
        {
            // Devolvemos la vista para crear un nuevo sistema
            return View();
        }

        // POST: SistemaController/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(SistemaViewModel sistema)
        {
            try
            {
                // Si el modelo es válido, llamamos al método InsertarSistemaAsync
                await _daoSistemas.InsertarSistemaAsync(sistema);
                // Redirigimos a la acción Index después de insertar el sistema
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {   // En caso de error, devolvemos la vista con un mensaje de error
                ViewBag.Error = "Error al insertar el sistema: " + e.Message;
                return View(sistema);
            }
        }


    }
}
