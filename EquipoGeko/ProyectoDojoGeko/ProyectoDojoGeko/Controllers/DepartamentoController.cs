using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class DepartamentoController : Controller
    {
        private readonly daoDepartamentoWSAsync _dao;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public DepartamentoController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoDepartamentoWSAsync(connectionString);
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
                var departamentos = await _dao.ObtenerDepartamentosAsync();
                return View(departamentos);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Index Departamento", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(DepartamentoViewModel departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.InsertarDepartamentoAsync(departamento);
                    await RegistrarLogYBitacora("Crear Departamento", $"Departamento '{departamento.NombreDepartamento}' creado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Crear Departamento", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var departamento = await _dao.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                    return NotFound();

                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Departamento (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(DepartamentoViewModel departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.ActualizarDepartamentoAsync(departamento);
                    await RegistrarLogYBitacora("Editar Departamento", $"Departamento '{departamento.NombreDepartamento}' actualizado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Departamento (POST)", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var departamento = await _dao.ObtenerDepartamentoPorIdAsync(id);
                if (departamento == null)
                    return NotFound();

                return View(departamento);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Departamento (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarDepartamentoAsync(id);
                await RegistrarLogYBitacora("Eliminar Departamento", $"Departamento con ID {id} desactivado.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Departamento (POST)", ex.Message);
                return View("Error");
            }
        }
    }
}
