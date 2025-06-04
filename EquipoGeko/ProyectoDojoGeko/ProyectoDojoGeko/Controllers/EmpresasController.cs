using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class EmpresaController : Controller
    {
        private readonly daoEmpresaWSAsync _dao;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public EmpresaController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _dao = new daoEmpresaWSAsync(connectionString);
            _daoLog = new daoLogWSAsync(connectionString);
            _daoBitacoraWS = new daoBitacoraWSAsync(connectionString);
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var empresas = await _dao.ObtenerEmpresasAsync();
                return View(empresas);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(EmpresaViewModel empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.InsertarEmpresaAsync(empresa);
                    return RedirectToAction(nameof(Index));
                }
                return View(empresa);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var empresa = await _dao.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                    return NotFound();

                return View(empresa);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EmpresaViewModel empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.ActualizarEmpresaAsync(empresa);
                    return RedirectToAction(nameof(Index));
                }
                return View(empresa);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var empresa = await _dao.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                    return NotFound();

                return View(empresa);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarEmpresaAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
