using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class EmpresasController : Controller
    {
        private readonly daoEmpresaWSAsync _daoEmpresa;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public EmpresasController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoEmpresa = new daoEmpresaWSAsync(connectionString);
            _daoLog = new daoLogWSAsync(connectionString);
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);
        }

        // Método privado para registrar errores en Log
        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Desconocido";
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = accion,
                Descripcion = $"Error por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método para obtener la lista de empresas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Desconocido";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Empresas",
                    Descripcion = $"Ingreso a la vista de empresas exitoso por {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(empresas ?? new List<EmpresaViewModel>());
            }
            catch (Exception ex)
            {
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Desconocido";

                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Vista Empresas",
                    Descripcion = $"Error en vista empresas por {usuario}: {ex.Message}",
                    Estado = false
                });

                ViewBag.Error = "Error al conectar con la base de datos.";
                return View(new List<EmpresaViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new EmpresaViewModel());
        }

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
                ViewBag.Error = "Error al crear la empresa.";
                return View(empresa);
            }
        }

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
                await RegistrarError("Error obtener empresa para editar", ex);
                return RedirectToAction("Index");
            }
        }

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
                ViewBag.Error = "Error al actualizar la empresa.";
                return View(empresa);
            }
        }

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
                await RegistrarError("Error obtener detalles de empresa", ex);
                return RedirectToAction("Index");
            }
        }

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
                await RegistrarError("Error al listar empresa", ex);
                return RedirectToAction("Index");
            }
        }

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
                TempData["ErrorMessage"] = "Error al eliminar la empresa.";
                await RegistrarError("Error eliminar empresa", ex);
                return RedirectToAction("Index");
            }
        }


    }
}