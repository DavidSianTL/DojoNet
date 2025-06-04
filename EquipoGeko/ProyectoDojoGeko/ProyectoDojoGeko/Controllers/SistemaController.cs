using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemaController : Controller
    {
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public SistemaController()
        {
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoSistema = new daoSistemaWSAsync(_connectionString);
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
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = accion,
                    Descripcion = descripcion,
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });
            }
            catch { }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var sistemas = await _daoSistema.ObtenerSistemasAsync();
                return View(sistemas);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Index Sistema", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(SistemaViewModel sistema)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoSistema.InsertarSistemaAsync(sistema);
                    return RedirectToAction(nameof(Index));
                }
                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Crear Sistema", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                    return NotFound();

                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Sistema (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SistemaViewModel sistema)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoSistema.ActualizarSistemaAsync(sistema);
                    return RedirectToAction(nameof(Index));
                }
                return View(sistema);
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
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                    return NotFound();

                return View(sistema);
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
                await _daoSistema.EliminarSistemaAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}

