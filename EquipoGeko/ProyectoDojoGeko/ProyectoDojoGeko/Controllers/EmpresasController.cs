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

        // Método para obtener la lista de empresas
        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();

                await RegistrarBitacora("Vista Empresas",
                    "Acceso exitoso a la lista de empresas");

                return View(empresas ?? new List<EmpresaViewModel>());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de empresas", ex);
                ViewBag.Error = "Error al cargar la lista de empresas.";
                return View(new List<EmpresaViewModel>());
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                await RegistrarBitacora("Vista Crear Empresa",
                    "Acceso a la vista de creación de empresa");
                return View(new EmpresaViewModel());
            }
            catch (Exception ex)
            {
                await RegistrarError("acceder a la vista de creación de empresa", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(EmpresaViewModel empresa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RegistrarError("crear empresa - datos inválidos",
                        new Exception("Validación de modelo fallida"));
                    return View(empresa);
                }

                var idEmpresa = await _daoEmpresa.InsertarEmpresaAsync(empresa);

                await RegistrarBitacora("Crear Empresa",
                    $"Nueva empresa creada: {empresa.Nombre} (ID: {idEmpresa})");

                TempData["SuccessMessage"] = "Empresa creada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("crear empresa", ex);
                ViewBag.Error = "Error al crear la empresa.";
                return View(empresa);
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                {
                    await RegistrarError("editar empresa - no encontrada",
                        new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }

                await RegistrarBitacora("Vista Editar Empresa",
                    $"Acceso a edición de empresa: {empresa.Nombre} (ID: {id})");

                return View(empresa);
            }
            catch (Exception ex)
            {
                await RegistrarError("obtener empresa para editar", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EmpresaViewModel empresa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await RegistrarError("actualizar empresa - datos inválidos",
                        new Exception("Validación de modelo fallida"));
                    return View(empresa);
                }

                await _daoEmpresa.ActualizarEmpresaAsync(empresa);

                await RegistrarBitacora("Actualizar Empresa",
                    $"Empresa actualizada: {empresa.Nombre} (ID: {empresa.IdEmpresa})");

                TempData["SuccessMessage"] = "Empresa actualizada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("actualizar empresa", ex);
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
                if (empresa == null)
                {
                    await RegistrarError("ver detalles de empresa - no encontrada",
                        new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }

                await RegistrarBitacora("Ver Detalles Empresa",
                    $"Visto detalle de empresa: {empresa.Nombre} (ID: {id})");

                return View(empresa);
            }
            catch (Exception ex)
            {
                await RegistrarError("obtener detalles de empresa", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar(int id)
        {
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                {
                    await RegistrarError("listar empresa - no encontrada",
                        new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }

                await RegistrarBitacora("Listar Empresa",
                    $"Empresa listada: {empresa.Nombre} (ID: {id})");

                return View(empresa);
            }
            catch (Exception ex)
            {
                await RegistrarError("listar empresa", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var empresa = await _daoEmpresa.ObtenerEmpresaPorIdAsync(id);
                if (empresa == null)
                {
                    await RegistrarError("eliminar empresa - no encontrada",
                        new Exception($"Empresa con ID {id} no encontrada"));
                    return NotFound();
                }

                await _daoEmpresa.EliminarEmpresaAsync(id);

                await RegistrarBitacora("Eliminar Empresa",
                    $"Empresa eliminada: {empresa.Nombre} (ID: {id})");

                TempData["SuccessMessage"] = "Empresa eliminada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await RegistrarError("eliminar empresa", ex);
                TempData["ErrorMessage"] = "Error al eliminar la empresa.";
                return RedirectToAction("Index");
            }
        }
    }
}