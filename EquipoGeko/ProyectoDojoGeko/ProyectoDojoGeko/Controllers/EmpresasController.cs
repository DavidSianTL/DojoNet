using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly string connectionString;
        private readonly daoEmpresaWSAsync _daoEmpresa;

        public EmpresasController()
        {
            // Cadena de conexión a la base de datos
            connectionString = "Server=DESKTOP-LPDU6QD\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _daoEmpresa = new daoEmpresaWSAsync(connectionString);
        }

        // Vista principal que muestra lista de empresas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();
                ;

                if (empresas != null && empresas.Any())
                    return View(empresas);

                return View(new List<EmpresaViewModel>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empresas: {ex.Message}");
                ViewBag.Error = "Error al conectar con la base de datos.";
                return View(new List<EmpresaViewModel>());
            }
        }

        // Vista para crear nueva empresa (GET)
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new EmpresaViewModel());
        }

        // Crear empresa (POST)
        [HttpPost]
        public async Task<IActionResult> Crear(EmpresaViewModel empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoEmpresa.InsertarEmpresaAsync(empresa);
                    return RedirectToAction("Index");
                }
                return View(empresa);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear empresa: {ex.Message}");
                ViewBag.Error = "Error al crear la empresa.";
                return View(empresa);
            }
        }

        // Vista para editar empresa (GET)
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa != null)
                    return View(empresa);

                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empresa para editar: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Editar empresa (POST)
        [HttpPost]
        public async Task<IActionResult> Editar(EmpresaViewModel empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoEmpresa.ActualizarEmpresaAsync(empresa);
                    return RedirectToAction("Index");
                }
                return View(empresa);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar empresa: {ex.Message}");
                ViewBag.Error = "Error al actualizar la empresa.";
                return View(empresa);
            }
        }

        // Vista para ver detalles de una empresa
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa != null)
                    return View(empresa);

                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener detalles de empresa: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Vista para listar empresa (similar a detalle, pero con otro nombre si quieres)
        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa != null)
                    return View(empresa);

                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar empresa: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Acción para eliminar empresa
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _daoEmpresa.EliminarEmpresaAsync(id);
                TempData["SuccessMessage"] = "Empresa eliminada correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empresa: {ex.Message}");
                TempData["ErrorMessage"] = "Error al eliminar la empresa.";
                return RedirectToAction("Index");
            }
        }
    }
}
