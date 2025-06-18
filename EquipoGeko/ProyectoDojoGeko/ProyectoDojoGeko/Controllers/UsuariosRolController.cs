using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.RolPermisos;
using ProyectoDojoGeko.Models.Usuario;
using System.Data;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
	public class UsuariosRolController : Controller
	{
		// Cadena de conexión a la base de datos
		private readonly string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
		private readonly daoUsuariosRolWSAsync _daoUsuariosRol;
		private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoLogWSAsync _daoLog;
		private readonly daoBitacoraWSAsync _daoBitacora;

		public UsuariosRolController()
		{
			// Inicializamos el DAO con la cadena de conexión
			_daoUsuariosRol = new daoUsuariosRolWSAsync(_connectionString);
			_daoLog = new daoLogWSAsync(_connectionString);
			_daoBitacora = new daoBitacoraWSAsync(_connectionString);
            _daoUsuario = new daoUsuarioWSAsync(_connectionString);
            _daoRoles = new daoRolesWSAsync(_connectionString);
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
        [AuthorizeRole("SuperAdministrador", "Administrador","Visualizador","Editor")]
        // Método para obtener la lista de UsuariosRol
        public async Task<IActionResult> Index()
        {
            // Obtiene la lista de UsuariosRol desde el DAO
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
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        // Método para obtener un UsuarioRol por ID
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
                await RegistrarError($"Ejecutar Obtener UsuarioRol por IdUsuariosRol con IdUsuariosRol: {IdUsuariosRol}", ex);
                throw new Exception("Error al obtener el usuarioRol por ID", ex);
            }
            return View(nameof(Index), usuariosRolList);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Visualizador", "Editor")]
        // Método para obtener UsuariosRol por ID de Rol
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
                await RegistrarError($"Obtener UsuariosRol por FK_IdRol con el FK_IdRol: {FK_IdRol}", ex);
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
                await RegistrarError($"Ejecutar Obtener UsuariosRol por FK_IdUsuario con el FK_IdUsuario: {FK_IdUsuario}", ex);
                throw new Exception("Error al obtener el usuario y rol por ID de usuario");
            }
            return View(nameof(Index), usuariosRolList);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador","Editor")]
        public async Task<IActionResult> Crear()
        {
            var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
            var roles = await _daoRoles.ObtenerRolesAsync();

            // Preparamos el modelo para la vista
            var model = new UsuariosRolFormViewModel
            {
                // Asignamos la lista de usuarios para el dropdown
                Usuarios = usuarios?.Select(u => new SelectListItem
                {
                    Value = u.IdUsuario.ToString(),
                    Text = u.Username
                }).ToList() ?? new List<SelectListItem>(),

                // Asignamos la lista de roles para el dropdown
                Roles = roles?.Select(r => new SelectListItem
                {
                    Value = r.IdRol.ToString(),
                    Text = r.NombreRol
                }).ToList() ?? new List<SelectListItem>()
            };

            if (model.Usuarios.Count == 0 || model.Roles.Count == 0)
            {
                TempData["ErrorMessage"] = "No hay usuarios o roles disponibles para asignar.";
                return RedirectToAction(nameof(Index));
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
                    // Preparamos el modelo para la vista si hay errores de validación
                    var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
                    var roles = await _daoRoles.ObtenerRolesAsync();

                    var viewModel = new UsuariosRolFormViewModel
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

                    foreach (int rolesId in model.FK_IdsRol)
                    {
                        // Convertimos UsuariosRolFormViewModel a UsuariosRolViewModel
                        var nuevoRolUsuario = new UsuariosRolViewModel
                        {
                            FK_IdUsuario = model.FK_IdUsuario,
                            FK_IdRol = rolesId // Asignamos el ID del rol directamente
                        };

                        await _daoUsuariosRol.InsertarUsuarioRolAsync(nuevoRolUsuario);
                    }
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
            var usuariosRol = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(IdUsuariosRol);
                if (usuariosRol == null)
                {
                    TempData["ErrorMessage"] = "No se encontró el UsuarioRol con el ID proporcionado.";
                }
                else
                {
                    await RegistrarBitacora("Vista Editar UsuarioRol", $"Acceso exitoso a la vista de edición de UsuarioRol con ID: {IdUsuariosRol}");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError($"Ejecutar Obtener UsuarioRol por IdUsuariosRol con IdUsuariosRol: {IdUsuariosRol}", ex);
            }
            return View(usuariosRol);
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
                        await RegistrarBitacora("Actualizar UsuarioRol", "UsuarioRol actualizado exitosamente");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo actualizar el UsuarioRol.");
                    }
                }
                catch (Exception ex)
                {
                    await RegistrarError($"Actualizar UsuariosRol con IdUsuariosRol: {usuarioRol.IdUsuarioRol}", ex);
                    ModelState.AddModelError("", $"Error al actualizar el UsuarioRol: {ex.Message}");
                }
            }
            return View(usuarioRol);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EliminarUsuarioRol(int IdUsuariosRol)
        {
            var usuariosRolList = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(IdUsuariosRol);
                var UsuariosRol = usuariosRolList.FirstOrDefault(usrsRl => usrsRl.IdUsuarioRol == IdUsuariosRol);
                if (usuariosRolList == null)
                {
                    TempData["ErrorMessage"] = "No se encontró el UsuarioRol con el ID proporcionado.";
                }
                else
                {
                    await RegistrarBitacora("Vista Eliminar UsuarioRol", $"Acceso exitoso a la vista de eliminación de UsuarioRol con ID: {IdUsuariosRol}");
                    return View(UsuariosRol);
                }
            }
            catch (Exception ex)
            {
                await RegistrarError($"Obtener UsuariosRol por IdUsuariosRol con IdUsuariosRol: {IdUsuariosRol}", ex);
            }
            return View(usuariosRolList);
        }

        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> EliminarUsuarioRol(UsuariosRolViewModel UsuariosRol)
        {
            try
            {
                bool resultado = await _daoUsuariosRol.EliminarUsuarioRolAsync(UsuariosRol.IdUsuarioRol);
                if (resultado)
                {
                    await RegistrarBitacora("Eliminar UsuarioRol", "UsuarioRol eliminado exitosamente");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo eliminar el UsuarioRol.");
                }
            }
            catch (Exception ex)
            {
                await RegistrarError("Eliminar UsuariosRol", ex);
                ModelState.AddModelError("", $"Error al eliminar el UsuarioRol: {ex.Message}");
            }
            return View(UsuariosRol);
        }

    }
}


