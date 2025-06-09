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
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método privado para registrar acciones exitosas en Bitácora
        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        // Vista principal que muestra lista de sistemas
        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var sistemas = await _daoSistema.ObtenerSistemasAsync();

                await RegistrarBitacora("Vista Sistemas",
                    "Acceso exitoso a la lista de sistemas");

                return View(sistemas ?? new List<SistemaViewModel>());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de sistemas", ex);
                ViewBag.Error = "Error al cargar la lista de sistemas.";
                return View(new List<SistemaViewModel>());
            }
        }

        // Vista para crear nuevo sistema (GET)
        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                await RegistrarBitacora("Vista Crear Sistema",
                    "Acceso a la vista de creación de sistema");
                return View(new SistemaViewModel());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de sistema", ex);
                return RedirectToAction("Index");
            }
        }

        // Crear sistema (POST)
        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(SistemaViewModel sistema)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RegistrarError("crear sistema - datos inválidos",
                        new Exception("Validación de modelo fallida"));
                    return View(sistema);
                }

               await _daoSistema.InsertarSistemaAsync(sistema);

                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await RegistrarBitacora("Crear Sistema",
                    $"Nuevo sistema creado: {sistema.Nombre} (ID: {idSistema})");

                TempData["SuccessMessage"] = "Sistema creado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("crear sistema", ex);
                ViewBag.Error = "Error al crear el sistema.";
                return View(sistema);
            }
        }

        // Vista para editar sistema (GET)
        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("editar sistema - no encontrado",
                        new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Vista Editar Sistema",
                    $"Acceso a edición de sistema: {sistema.Nombre} (ID: {id})");

                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarError("obtener sistema para editar", ex);
                return RedirectToAction("Index");
            }
        }

        // Editar sistema (POST)
        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(SistemaViewModel sistema)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RegistrarError("actualizar sistema - datos inválidos",
                        new Exception("Validación de modelo fallida"));
                    return View(sistema);
                }

                await _daoSistema.ActualizarSistemaAsync(sistema);

                await RegistrarBitacora("Actualizar Sistema",
                    $"Sistema actualizado: {sistema.Nombre} (ID: {sistema.IdSistema})");

                TempData["SuccessMessage"] = "Sistema actualizado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("actualizar sistema", ex);
                ViewBag.Error = "Error al actualizar el sistema.";
                return View(sistema);
            }
        }

        // Vista para ver detalles de un sistema
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("ver detalles de sistema - no encontrado",
                        new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Ver Detalles Sistema",
                    $"Visto detalle de sistema: {sistema.Nombre} (ID: {id})");

                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarError("obtener detalles de sistema", ex);
                return RedirectToAction("Index");
            }
        }

        // Vista para listar sistema
        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("listar sistema - no encontrado",
                        new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }

                await RegistrarBitacora("Listar Sistema",
                    $"Sistema listado: {sistema.Nombre} (ID: {id})");

                return View(sistema);
            }
            catch (Exception ex)
            {
                await RegistrarError("listar sistema", ex);
                return RedirectToAction("Index");
            }
        }

        // Acción para eliminar sistema
        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var sistema = await _daoSistema.ObtenerSistemaPorIdAsync(id);
                if (sistema == null)
                {
                    await RegistrarError("eliminar sistema - no encontrado",
                        new Exception($"Sistema con ID {id} no encontrado"));
                    return NotFound();
                }

                await _daoSistema.EliminarSistemaAsync(id);

                await RegistrarBitacora("Eliminar Sistema",
                    $"Sistema eliminado: {sistema.Nombre} (ID: {id})");

                TempData["SuccessMessage"] = "Sistema eliminado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar sistema", ex);
                TempData["ErrorMessage"] = "Error al eliminar el sistema.";
                return RedirectToAction("Index");
            }
        }
    }
}