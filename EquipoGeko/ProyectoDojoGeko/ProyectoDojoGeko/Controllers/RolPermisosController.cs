using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.RolPermisos;

namespace ProyectoDojoGeko.Controllers
{
    public class RolPermisosController : Controller
    {
        private readonly string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        private readonly daoRolPermisosWSAsync _daoRolesPermisos;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoSistemaWSAsync _daoSistemas;
        private readonly daoPermisosWSAsync _daoPermisos;

        public RolPermisosController()
        {
            _daoRolesPermisos = new daoRolPermisosWSAsync(_connectionString);
            _daoLog = new daoLogWSAsync(_connectionString);
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            _daoRoles = new daoRolesWSAsync(_connectionString);
            _daoSistemas = new daoSistemaWSAsync(_connectionString);
            _daoPermisos = new daoPermisosWSAsync(_connectionString);
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

        [HttpGet]
        public async Task<IActionResult> DetallesRolesPermisos()
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosAsync();

                if (!rolPermisosList.Any())
                {
                    ViewBag.Mensaje = "No se encontraron roles y permisos. ";
                }

                await RegistrarBitacora("DetallesRolesPermisos", "Acceso a lista de roles y permisos");
            }
            catch (Exception ex)
            {
                await RegistrarError("DetallesRolesPermisos", ex);
                ViewBag.Mensaje = "Error al obtener los roles y permisos.";
            }

            return View(rolPermisosList);
        }

        [HttpGet]
        public async Task<IActionResult> RolPermisosPorIdRolPermiso(int IdRolPermisos)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);
                await RegistrarBitacora("RolPermisosPorIdRolPermiso", $"Consultado ID: {IdRolPermisos}");
            }
            catch (Exception ex)
            {
                await RegistrarError("RolPermisosPorIdRolPermiso", ex);
            }

            return View(nameof(DetallesRolesPermisos), rolPermisosList);
        }

        [HttpGet]
        public async Task<IActionResult> RolPermisosPorIdRol(int FK_IdRol)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolAsync(FK_IdRol);
                await RegistrarBitacora("RolPermisosPorIdRol", $"Consultado ID Rol: {FK_IdRol}");
            }
            catch (Exception ex)
            {
                await RegistrarError("RolPermisosPorIdRol", ex);
            }

            return View(nameof(DetallesRolesPermisos), rolPermisosList);
        }

        [HttpGet]
        public async Task<IActionResult> RolPermisosPorIdPermiso(int FK_IdPermiso)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdPermisoAsync(FK_IdPermiso);
                await RegistrarBitacora("RolPermisosPorIdPermiso", $"Consultado ID Permiso: {FK_IdPermiso}");
            }
            catch (Exception ex)
            {
                await RegistrarError("RolPermisosPorIdPermiso", ex);
            }

            return View(nameof(DetallesRolesPermisos), rolPermisosList);
        }

        [HttpGet]
        public async Task<IActionResult> RolPermisosPorIdSistema(int FK_IdSistema)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdSistemaAsync(FK_IdSistema);
                await RegistrarBitacora("RolPermisosPorIdSistema", $"Consultado ID Sistema: {FK_IdSistema}");
            }
            catch (Exception ex)
            {
                await RegistrarError("RolPermisosPorIdSistema", ex);
            }

            return View(nameof(DetallesRolesPermisos), rolPermisosList);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            try
            {
                var roles = await _daoRoles.ObtenerRolesAsync();
                var sistemas = await _daoSistemas.ObtenerSistemasAsync();
                var permisos = await _daoPermisos.ObtenerPermisosAsync();

                var model = new RolPermisosFormViewModel
                {
                    Roles = roles.Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList(),
                    Sistemas = sistemas.Select(s => new SelectListItem
                    {
                        Value = s.IdSistema.ToString(),
                        Text = s.Nombre
                    }).ToList(),
                    Permisos = permisos.Select(p => new SelectListItem
                    {
                        Value = p.IdPermiso.ToString(),
                        Text = p.NombrePermiso
                    }).ToList()
                };

                await RegistrarBitacora("CrearRolPermisos", "Acceso a formulario de creación");
                return View(model);
            }
            catch (Exception e)
            {
                await RegistrarError("CrearRolPermisos", e);
                return View(new RolPermisosFormViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear(RolPermisosFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Roles = (await _daoRoles.ObtenerRolesAsync()).Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList();

                    model.Sistemas = (await _daoSistemas.ObtenerSistemasAsync()).Select(s => new SelectListItem
                    {
                        Value = s.IdSistema.ToString(),
                        Text = s.Nombre
                    }).ToList();

                    model.Permisos = (await _daoPermisos.ObtenerPermisosAsync()).Select(p => new SelectListItem
                    {
                        Value = p.IdPermiso.ToString(),
                        Text = p.NombrePermiso
                    }).ToList();

                    return View(model);
                }

                foreach (int permisoId in model.FK_IdsPermisos)
                {
                    var nuevoRolPermiso = new RolPermisosViewModel
                    {
                        FK_IdRol = model.FK_IdRol,
                        FK_IdPermiso = permisoId,
                        FK_IdSistema = model.FK_IdSistema
                    };

                    await _daoRolesPermisos.InsertarRolPermisoAsync(nuevoRolPermiso);
                }

                await RegistrarBitacora("CrearRolPermisos", "Rol y permisos creados exitosamente");
                TempData["SuccessMessage"] = "Rol y permisos creados correctamente.";
                return RedirectToAction(nameof(Crear));
            }
            catch (Exception ex)
            {
                await RegistrarError("CrearRolPermisos", ex);
                ModelState.AddModelError(string.Empty, "Error al crear los permisos: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarRolPermisos(int IdRolPermisos)
        {
            var rolPermisos = new List<RolPermisosViewModel>();
            try
            {
                rolPermisos = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);
                await RegistrarBitacora("ActualizarRolPermisos", $"Acceso a edición ID: {IdRolPermisos}");
                return View(rolPermisos);
            }
            catch (Exception ex)
            {
                await RegistrarError("ActualizarRolPermisos", ex);
                return RedirectToAction(nameof(DetallesRolesPermisos));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarRolPermisos(RolPermisosViewModel rolPermisos)
        {
            try
            {
                if (!ModelState.IsValid) return View(rolPermisos);

                await _daoRolesPermisos.ActualizarRolPermisoAsync(rolPermisos);
                await RegistrarBitacora("ActualizarRolPermisos", $"Actualizado ID: {rolPermisos.IdRolPermiso}");
                TempData["SuccessMessage"] = "Rol y permiso actualizado correctamente.";
                return RedirectToAction(nameof(DetallesRolesPermisos));
            }
            catch (Exception ex)
            {
                await RegistrarError("ActualizarRolPermisos", ex);
                ModelState.AddModelError(string.Empty, "Error al actualizar el rol y permiso: " + ex.Message);
                return View(rolPermisos);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EliminarRolPermisos(int IdRolPermisos)
        {
            var rolPermisos = new List<RolPermisosViewModel>();
            try
            {
                rolPermisos = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);
                await RegistrarBitacora("EliminarRolPermisos", $"Acceso a eliminación ID: {IdRolPermisos}");
                return View(rolPermisos);
            }
            catch (Exception ex)
            {
                await RegistrarError("EliminarRolPermisos", ex);
                return RedirectToAction(nameof(DetallesRolesPermisos));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarRolPermisos(RolPermisosViewModel rolPermisos)
        {
            try
            {
                if (!ModelState.IsValid) return View(rolPermisos);

                await _daoRolesPermisos.EliminarRolPermisoAsync(rolPermisos.IdRolPermiso);
                await RegistrarBitacora("EliminarRolPermisos", $"Eliminado ID: {rolPermisos.IdRolPermiso}");
                TempData["SuccessMessage"] = "Rol y permiso eliminado correctamente.";
                return RedirectToAction(nameof(DetallesRolesPermisos));
            }
            catch (Exception ex)
            {
                await RegistrarError("EliminarRolPermisos", ex);
                ModelState.AddModelError(string.Empty, "Error al eliminar el rol y permiso: " + ex.Message);
                return View(rolPermisos);
            }
        }
    }
}