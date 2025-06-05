using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class DepartamentosController : Controller
    {
        private readonly daoDepartamentoWSAsync _daoDepartamento;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public DepartamentosController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoDepartamento = new daoDepartamentoWSAsync(connectionString);
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var departamentos = await _daoDepartamento.ObtenerDepartamentosAsync();

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Desconocido";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Departamentos",
                    Descripcion = $"Ingreso a la vista de departamentos exitoso por {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(departamentos ?? new List<DepartamentoViewModel>());
            }
            catch (Exception ex)
            {
                await RegistrarError("Error Vista Departamentos", ex);
                ViewBag.Error = "Error al conectar con la base de datos.";
                return View(new List<DepartamentoViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new DepartamentoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(DepartamentoViewModel departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoDepartamento.InsertarDepartamentoAsync(departamento);
                    return RedirectToAction("Index");
                }
                return View(departamento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear el departamento.";
                return View(departamento);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento != null)
                    return View(departamento);

                return NotFound();
            }
            catch (Exception ex)
            {
                await RegistrarError("Error obtener departamento para editar", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(DepartamentoViewModel departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoDepartamento.ActualizarDepartamentoAsync(departamento);
                    return RedirectToAction("Index");
                }
                return View(departamento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar el departamento.";
                return View(departamento);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento != null)
                    return View(departamento);

                return NotFound();
            }
            catch (Exception ex)
            {
                await RegistrarError("Error obtener detalles de departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            try
            {
                var departamento = await _daoDepartamento.ObtenerDepartamentoPorIdAsync(id);
                if (departamento != null)
                    return View(departamento);

                return NotFound();
            }
            catch (Exception ex)
            {
                await RegistrarError("Error al listar departamento", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _daoDepartamento.EliminarDepartamentoAsync(id);
                TempData["SuccessMessage"] = "Departamento eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el departamento.";
                await RegistrarError("Error eliminar departamento", ex);
                return RedirectToAction("Index");
            }
        }

        
    }
}