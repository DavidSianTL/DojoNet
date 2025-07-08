using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.RolPermisos;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class UsuariosRolController : Controller
    {
        private readonly daoUsuariosRolWSAsync _daoUsuariosRol;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly ILoggingService _loggingService;

        public UsuariosRolController(
            daoUsuariosRolWSAsync daoUsuariosRol,
            daoUsuarioWSAsync daoUsuario,
            daoRolesWSAsync daoRoles,
            daoBitacoraWSAsync daoBitacora,
            ILoggingService loggingService)
        {
            _daoUsuariosRol = daoUsuariosRol;
            _daoUsuario = daoUsuario;
            _daoRoles = daoRoles;
            _daoBitacora = daoBitacora;
            _loggingService = loggingService;
        }

        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _loggingService.RegistrarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

        /*[HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Visualizador", "Editor")]
        public async Task<IActionResult> Index()
        {
            var usuariosRolList = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolAsync();
                if (!usuariosRolList.Any())
                {
                    ViewBag.Mensaje = "No se encontraron usuarios y roles.";
                }
                else
                {
                    await RegistrarBitacora("Vista UsuariosRol", "Acceso exitoso a la lista de UsuariosRol");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("Ejecutar Obtener UsuariosRol", ex);
                throw new Exception("Error al obtener la lista de UsuariosRol", ex);
            }
            return View(usuariosRolList);
        }*/

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public IActionResult Index()
        {
            ViewBag.Title = "UsuariosRol - En Construcción";
            return View();
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> UsuarioRolPorId(int IdUsuariosRol)
        {
            var usuariosRolList = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(IdUsuariosRol);
                if (!usuariosRolList.Any())
                {
                    ViewBag.Mensaje = "No se encontró el usuarioRol con el ID proporcionado.";
                }
                else
                {
                    await RegistrarBitacora("Obtener UsuarioRol por ID", $"UsuarioRol encontrado con ID: {IdUsuariosRol}");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("Obtener UsuarioRol por ID", ex);
                throw new Exception("Error al obtener el usuarioRol por ID", ex);
            }
            return View(nameof(Index), usuariosRolList);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Visualizador", "Editor")]
        public async Task<IActionResult> UsuarioRolPorIdRol(int FK_IdRol)
        {
            var usuariosRolList = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdRolAsync(FK_IdRol);
                if (!usuariosRolList.Any())
                {
                    ViewBag.Mensaje = "No se encontraron usuarios y roles para el ID de rol proporcionado.";
                }
                else
                {
                    await RegistrarBitacora("Obtener UsuarioRol por ID de Rol", $"UsuariosRol encontrados para el ID de rol: {FK_IdRol}");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("Obtener UsuarioRol por ID de Rol", ex);
                throw new Exception("Error al obtener el usuario y rol por ID de rol");
            }
            return View(nameof(Index), usuariosRolList);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Visualizador", "Editor")]
        public async Task<IActionResult> UsuarioRolPorIdUsuario(int FK_IdUsuario)
        {
            var usuariosRolList = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdUsuarioAsync(FK_IdUsuario);
                if (!usuariosRolList.Any())
                {
                    ViewBag.Mensaje = "No se encontraron usuarios y roles para el ID de usuario proporcionado.";
                }
                else
                {
                    await RegistrarBitacora("Obtener UsuarioRol por ID de Usuario", $"UsuariosRol encontrados para el ID de usuario: {FK_IdUsuario}");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("Obtener UsuarioRol por ID de Usuario", ex);
                throw new Exception("Error al obtener el usuario y rol por ID de usuario");
            }
            return View(nameof(Index), usuariosRolList);
        }
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
            var roles = await _daoRoles.ObtenerRolesAsync();

            var model = new UsuariosRolFormViewModel
            {
                Usuarios = usuarios?.Select(u => new SelectListItem
                {
                    Value = u.IdUsuario.ToString(),
                    Text = u.Username
                }).ToList() ?? new List<SelectListItem>(),

                Roles = roles?.Select(r => new SelectListItem
                {
                    Value = r.IdRol.ToString(),
                    Text = r.NombreRol
                }).ToList() ?? new List<SelectListItem>()
            };

            if (model.Usuarios.Count == 0 || model.Roles.Count == 0)
            {
                TempData["ErrorMessage"] = "No hay usuarios o roles disponibles para asignar.";
                return View(new List<UsuariosRolFormViewModel>());
            }

            await RegistrarBitacora("Vista Insertar UsuarioRol", "Acceso exitoso a la vista de inserción de UsuarioRol");
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(UsuariosRolFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (int rolesId in model.FK_IdsRol)
                    {
                        var nuevoRolUsuario = new UsuariosRolViewModel
                        {
                            FK_IdUsuario = model.FK_IdUsuario,
                            FK_IdRol = rolesId
                        };

                        await _daoUsuariosRol.InsertarUsuarioRolAsync(nuevoRolUsuario);
                    }

                    await RegistrarBitacora("Insertar UsuarioRol", $"Usuario {model.FK_IdUsuario} asignado a roles: {string.Join(",", model.FK_IdsRol)}");
                    return RedirectToAction(nameof(Crear));
                }
                catch (Exception ex)
                {
                    await RegistrarError("Insertar UsuariosRol", ex);
                    ModelState.AddModelError("", $"Error al insertar el UsuarioRol: {ex.Message}");
                }
            }
            return View(model);
        }
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EditarUsuarioRol(int IdUsuariosRol)
        {
            try
            {
                var usuariosRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(IdUsuariosRol);
                if (usuariosRol == null || !usuariosRol.Any())
                {
                    TempData["ErrorMessage"] = "No se encontró el UsuarioRol con el ID proporcionado.";
                    return RedirectToAction(nameof(Index));
                }

                await RegistrarBitacora("Vista Editar UsuarioRol", $"Acceso exitoso a la vista de edición de UsuarioRol con ID: {IdUsuariosRol}");
                return View(usuariosRol.First());
            }
            catch (Exception ex)
            {
                await RegistrarError("Editar UsuarioRol", ex);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EditarUsuarioRol(UsuariosRolViewModel usuarioRol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool resultado = await _daoUsuariosRol.ActualizarUsuarioRolAsync(usuarioRol);
                    if (resultado)
                    {
                        await RegistrarBitacora("Actualizar UsuarioRol", $"UsuarioRol actualizado: ID {usuarioRol.IdUsuarioRol}");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo actualizar el UsuarioRol.");
                    }
                }
                catch (Exception ex)
                {
                    await RegistrarError("Actualizar UsuarioRol", ex);
                    ModelState.AddModelError("", $"Error al actualizar el UsuarioRol: {ex.Message}");
                }
            }
            return View(usuarioRol);
        }
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EliminarUsuarioRol(int IdUsuariosRol)
        {
            try
            {
                var usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(IdUsuariosRol);
                var usuarioRol = usuariosRolList.FirstOrDefault(usrsRl => usrsRl.IdUsuarioRol == IdUsuariosRol);

                if (usuarioRol == null)
                {
                    TempData["ErrorMessage"] = "No se encontró el UsuarioRol con el ID proporcionado.";
                    return RedirectToAction(nameof(Index));
                }

                await RegistrarBitacora("Vista Eliminar UsuarioRol", $"Acceso exitoso a la vista de eliminación de UsuarioRol con ID: {IdUsuariosRol}");
                return View(usuarioRol);
            }
            catch (Exception ex)
            {
                await RegistrarError("Cargar vista Eliminar UsuarioRol", ex);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EliminarUsuarioRol(UsuariosRolViewModel usuarioRol)
        {
            try
            {
                bool resultado = await _daoUsuariosRol.EliminarUsuarioRolAsync(usuarioRol.IdUsuarioRol);
                if (resultado)
                {
                    await RegistrarBitacora("Eliminar UsuarioRol", $"UsuarioRol eliminado: ID {usuarioRol.IdUsuarioRol}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo eliminar el UsuarioRol.");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("Eliminar UsuarioRol", ex);
                ModelState.AddModelError("", $"Error al eliminar el UsuarioRol: {ex.Message}");
            }
            return View(usuarioRol);
        }
    }
}
