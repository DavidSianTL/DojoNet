using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class SistemaController : Controller
    {
        private readonly daoSistemaWSAsync _dao;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public SistemaController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoSistemaWSAsync(connectionString);
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
            catch { }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var sistemas = await _dao.ObtenerSistemasAsync();
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
                    await _dao.InsertarSistemaAsync(sistema);
                    await RegistrarLogYBitacora("Crear Sistema", $"Sistema '{sistema.NombreSistema}' creado.");
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
                var sistema = await _dao.ObtenerSistemaPorIdAsync(id);
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
                    await _dao.ActualizarSistemaAsync(sistema);
                    await RegistrarLogYBitacora("Editar Sistema", $"Sistema '{sistema.NombreSistema}' actualizado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Sistema (POST)", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var sistema = await _dao.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                    return NotFound();

                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Sistema (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarSistemaAsync(id);
                await RegistrarLogYBitacora("Eliminar Sistema", $"Sistema con ID {id} desactivado.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Sistema (POST)", ex.Message);
                return View("Error");
            }
        }
    }
}

