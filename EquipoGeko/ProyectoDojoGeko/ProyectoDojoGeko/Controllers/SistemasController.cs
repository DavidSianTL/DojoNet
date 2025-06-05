using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemasController : Controller
    {
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        public SistemasController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _daoSistema = new daoSistemaWSAsync(connectionString);
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
                var sistemas = await _daoSistema.ObtenerSistemasAsync();

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Desconocido";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Sistemas",
                    Descripcion = $"Ingreso a la vista de sistemas exitoso por {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(sistemas ?? new List<SistemaViewModel>());
            }
            catch (Exception ex)
            {
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Desconocido";

                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Vista Sistemas",
                    Descripcion = $"Error en vista sistemas por {usuario}: {ex.Message}",
                    Estado = false
                });

                ViewBag.Error = "Error al conectar con la base de datos.";
                return View(new List<SistemaViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new SistemaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SistemaViewModel sistema)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoSistema.InsertarSistemaAsync(sistema);
                    return RedirectToAction("Index");
                }
                return View(sistema);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear el sistema.";
                return View(sistema);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema != null)
                    return View(sistema);

                return NotFound();
            }
            catch (Exception ex)
            {
                await RegistrarError("Error obtener sistema para editar", ex);
                return RedirectToAction("Index");
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
                    return RedirectToAction("Index");
                }
                return View(sistema);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar el sistema.";
                return View(sistema);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema != null)
                    return View(sistema);

                return NotFound();
            }
            catch (Exception ex)
            {
                await RegistrarError("Error obtener detalles de sistema", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema != null)
                    return View(sistema);

                return NotFound();
            }
            catch (Exception ex)
            {
                await RegistrarError("Error al listar sistema", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _daoSistema.EliminarSistemaAsync(id);
                TempData["SuccessMessage"] = "Sistema eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el sistema.";
                await RegistrarError("Error eliminar sistema", ex);
                return RedirectToAction("Index");
            }
        }

        
    }
}