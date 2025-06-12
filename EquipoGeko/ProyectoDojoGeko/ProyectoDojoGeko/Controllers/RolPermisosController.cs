using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using System.Globalization;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
	public class RolPermisosController : Controller
	{
		private readonly string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
		private readonly daoRolPermisosWSAsync _daoRolesPermisos;
		private readonly daoLogWSAsync _daoLog;
		private readonly daoBitacoraWSAsync _daoBitacora;

		public RolPermisosController()
		{
			_daoRolesPermisos = new daoRolPermisosWSAsync(_connectionString);
			_daoLog = new daoLogWSAsync(_connectionString);
			_daoBitacora = new daoBitacoraWSAsync(_connectionString);

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


		#region Métodos tipo SELECT



		[HttpGet]
		public async Task<IActionResult> DetallesRolesPermisos()
		{
			var rolPermisosList = new List<RolPermisosViewModel>();
			try
			{
				rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosAsync();

				if (!rolPermisosList.Any())
				{
					ViewBag.Mensaje ="No se encontraron roles y permisos. ";
				}
			}
			catch (Exception ex)
			{
				await RegistrarError("Obtener lista de RolPermisos", ex);
				throw new Exception("Error al obtener los RolPermisos. ", ex);
			}

            // Registrar la acción exitosa en Bitácora
			await RegistrarBitacora("Ver Detalles de Roles y Permisos", "Se accedió a la lista de roles y permisos.");
            return View(rolPermisosList);

		}

		

		[HttpGet]
		public async Task<IActionResult> RolPermisosPorIdRolPermiso(int IdRolPermisos) // Busca por ID de RolPermiso (IdRolPermisos)
		{
			var rolPermisosList = new List<RolPermisosViewModel>();
			try
			{
				rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);
			}
			catch (Exception ex)
			{

				await RegistrarError($"Obtener RolPermisos por IdRolPermisos con IdRolPermisos: {IdRolPermisos}", ex);
				throw new Exception("Error al obtener los roles y permisos por IdRolPermisos", ex);
			}
            // Registrar la acción exitosa en Bitácora
			await RegistrarBitacora($"Ver RolPermisos por IdRolPermisos: {IdRolPermisos}", "Se accedió a los detalles de un rol y permiso específico.");
            return View(nameof(DetallesRolesPermisos), rolPermisosList);
		}



		[HttpGet]
		public async Task<IActionResult> RolPermisosPorIdRol(int FK_IdRol) // Busca por ID de Rol (FK_IdRol)
		{
			var rolPermisosList = new List<RolPermisosViewModel>();
			try
			{
				rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolAsync(FK_IdRol);
			}
			catch (Exception ex)
			{
				await RegistrarError($"Obtener RolPermisos por FK_IdRol con FK_IdRol: {FK_IdRol}", ex);
				throw new Exception("Error al obtener los roles y permisos por FK_IdRol", ex);
			}
            // Registrar la acción exitosa en Bitácora
			await RegistrarBitacora($"Ver RolPermisos por FK_IdRol: {FK_IdRol}", "Se accedió a los detalles de los roles y permisos asociados a un rol específico.");
            return View(nameof(DetallesRolesPermisos), rolPermisosList);
		}



		[HttpGet]
		public async Task<IActionResult> RolPermisosPorIdPermiso(int FK_IdPermiso) //Busca por ID de Permiso (FK_IdPermiso)
		{
			var rolPermisosList = new List<RolPermisosViewModel>();
			try
			{
				rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdPermisoAsync(FK_IdPermiso);
			}
			catch (Exception ex)
			{
				await RegistrarError($"Obtener RolPermisos por FK_IdPermiso con FK_IdPermiso: {FK_IdPermiso}", ex);
				throw new Exception("Error al obtener los roles y permisos por FK_IdPermiso", ex);
			}
            // Registrar la acción exitosa en Bitácora
			await RegistrarBitacora($"Ver RolPermisos por FK_IdPermiso", $"Se accedió a los detalles del RolPermisos con FK_IdPermiso: {FK_IdPermiso}.");
            return View(nameof(DetallesRolesPermisos), rolPermisosList);
		}



		[HttpGet]
		public async Task<IActionResult> RolPermisosPorIdSistema(int FK_IdSistema) // Busca por ID de Sistema (FK_IdSistema)
		{
			var rolPermisosList = new List<RolPermisosViewModel>();
			try
			{
				rolPermisosList = await _daoRolesPermisos.ObtenerRolPermisosPorIdSistemaAsync(FK_IdSistema);
			}
			catch (Exception ex)
			{
				await RegistrarError($"Obtener RolPermisos por FK_IdSistema con FK_IdSistema: {FK_IdSistema}", ex);
				throw new Exception("Error al obtener los roles y permisos por FK_IdSistema", ex);
			}
			// Registrar la acción exitosa en Bitácora
			await RegistrarBitacora("Ver RolPermisos por FK_IdSistema", $"Se accedió a la lista de usuarios con FK_IdSistema: {FK_IdSistema}");
            return View(nameof(DetallesRolesPermisos), rolPermisosList);
		}



		#endregion




		#region Métodos tipo INSERT, UPDATE y DELETE


		//											CREAR
		[HttpGet]
		public async Task<IActionResult> CrearRolPermisos()
		{
			await RegistrarBitacora("Crear RolPermisos", "Se accedió a la vista Crear RolPermisos");
            return View(new RolPermisosViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> CrearRolPermisos(RolPermisosViewModel rolPermisos)
		{
			if (!ModelState.IsValid) return View(rolPermisos);
			
			try
			{
				bool response = await _daoRolesPermisos.InsertarRolPermisoAsync(rolPermisos);

				if (response)
				{
					await RegistrarBitacora("Crear RolPermisos", "Rol y permiso creado correctamente.");
                    TempData["SuccessMessage"] = "Rol y permiso creado correctamente.";
					return RedirectToAction(nameof(DetallesRolesPermisos));
				}
				else
				{
					TempData["ErrorMessage"] = "No se pudo crear el rol y permiso.";
					return View(rolPermisos);
				}
								
			}
			catch (Exception ex)
			{
				await RegistrarError("Crear RolPermisos", ex);
				ModelState.AddModelError(string.Empty, "Error al crear el rol y permiso: " + ex.Message);
				return View(rolPermisos);
			}
		
			
		}



		//											ACTUALIZAR
		[HttpGet]
		public async Task<IActionResult> ActualizarRolPermisos(int IdRolPermisos)
		{
			var rolPermisos = new List<RolPermisosViewModel>();

			try
			{
				rolPermisos = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);

				if (rolPermisos == null)
				{
					TempData["ErrorMessage"] = "Rol y permiso no encontrado.";
					return RedirectToAction(nameof(DetallesRolesPermisos));
				}

			}catch (Exception ex)
			{
				await RegistrarError($"Actualizar RolPermisos con IdRolPermisos: {IdRolPermisos}", ex);
				return RedirectToAction(nameof(DetallesRolesPermisos));
            }


			await RegistrarBitacora("Editar Rolpermisos", $"Se accedió correctamente a la vista de editar RolPermisos");
			return View(rolPermisos);
		}

		[HttpPost]
		public async Task<IActionResult> ActualizarRolPermisos(RolPermisosViewModel rolPermisos)
		{
			if (!ModelState.IsValid) return View(rolPermisos);

			try
			{
				bool response = await _daoRolesPermisos.ActualizarRolPermisoAsync(rolPermisos);
				if (response)
				{
					await RegistrarBitacora("Editar RolPermisos", $"Se editó correctamente el RolPermiso con IdRolPermiso: {rolPermisos.IdRolPermiso}")
					TempData["SuccessMessage"] = "Rol y permiso actualizado correctamente.";
					return RedirectToAction(nameof(DetallesRolesPermisos));
				}
				else
				{
					TempData["ErrorMessage"] = "No se pudo actualizar el rol y permiso.";
					return View(rolPermisos);
				}

			}
			catch (Exception ex)
			{
				await RegistrarError($"Actualizar RolPermisos con IdRolPermisos: {rolPermisos.IdRolPermiso}", ex);
				ModelState.AddModelError(string.Empty, "Error al actualizar el rol y permiso: " + ex.Message);
				return View(rolPermisos);
			}
			
		}



		//										    ELIMINAR
		[HttpGet]
		public async Task<IActionResult> EliminarRolPermisos(int IdRolPermisos)
		{
			var rolPermisos = new List<RolPermisosViewModel>();
			try
			{
                rolPermisos = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);

                if (rolPermisos == null)
                {
                    TempData["ErrorMessage"] = "Rol y permiso no encontrado.";
                    return RedirectToAction(nameof(DetallesRolesPermisos));
                }
            }
			catch (Exception ex)
			{
				await RegistrarError($" Eliminar RolPermisos con IdRolPermisos: {IdRolPermisos}", ex);
				return RedirectToAction(nameof(DetallesRolesPermisos));
            }

			await RegistrarBitacora("Eliminar RolPermisos", $"Se accedió correctamente a la vista de eliminar RolPermisos");
            return View(rolPermisos);
		}

		[HttpPost]
		public async Task<IActionResult> EliminarRolPermisos(RolPermisosViewModel rolPermisos)
		{
			if (!ModelState.IsValid) return View(rolPermisos);
			try
			{
				bool response = await _daoRolesPermisos.EliminarRolPermisoAsync(rolPermisos.IdRolPermiso);

				if (response)
				{
					await RegistrarBitacora("Eliminar RolPermisos", $"RolPermiso con IdRolPermiso: {rolPermisos.IdRolPermiso} eliminado correctamente.");
                    TempData["SuccessMessage"] = "Rol y permiso eliminado correctamente.";
					return RedirectToAction(nameof(DetallesRolesPermisos));
				}
				else
				{
					TempData["ErrorMessage"] = "No se pudo eliminar el rol y permiso.";
					return View(rolPermisos);
				}
				
			}
			catch (Exception ex)
			{
				await RegistrarError($"Eliminar RolPermisos con IdRolPermisos: {rolPermisos.IdRolPermiso}", ex);
				ModelState.AddModelError(string.Empty, "Error al eliminar el rol y permiso: " + ex.Message);
				return View(rolPermisos);
			}
			
			
		}



		#endregion
	}
}