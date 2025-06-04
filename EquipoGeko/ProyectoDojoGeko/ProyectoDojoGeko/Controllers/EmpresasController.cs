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

        private async Task RegistrarLogYBitacora(string accion, string descripcion)
        {
            try
            {
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = accion,
                    Descripcion = descripcion,
                    Estado = true
                });

                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);
                var idSistema = rolesUsuario.FirstOrDefault()?.FK_IdSistema ?? 0;

                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = accion,
                    Descripcion = descripcion,
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });
            }
            catch
            {
                // Ignorar errores al registrar bitácora/logs
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var empresas = await _dao.ObtenerEmpresasAsync();
                return View(empresas);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Index Empresa", ex.Message);
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
                    await RegistrarLogYBitacora("Crear Empresa", $"Empresa '{empresa.NombreEmpresa}' creada.");
                    return RedirectToAction(nameof(Index));
                }
                return View(empresa);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Crear Empresa", ex.Message);
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
                await RegistrarLogYBitacora("Error Editar Empresa (GET)", ex.Message);
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
                    await RegistrarLogYBitacora("Editar Empresa", $"Empresa '{empresa.NombreEmpresa}' actualizada.");
                    return RedirectToAction(nameof(Index));
                }
                return View(empresa);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Empresa (POST)", ex.Message);
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
                await RegistrarLogYBitacora("Error Eliminar Empresa (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarEmpresaAsync(id);
                await RegistrarLogYBitacora("Eliminar Empresa", $"Empresa con ID {id} desactivada.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Empresa (POST)", ex.Message);
                return View("Error");
            }
        }
    }
}
