using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
	public class RolPermisosController : Controller
	{

		private readonly daoRolPermisosWSAsync _daoRolesPermisos;
		public RolPermisosController()
		{
			string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

			_daoRolesPermisos = new daoRolPermisosWSAsync(_connectionString);
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
				throw new Exception("Error al obtener los RolPermisos. ", ex);
			}
			   
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
				throw new Exception("Error al obtener los roles y permisos por IdRolPermisos", ex);
			}
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
				throw new Exception("Error al obtener los roles y permisos por FK_IdRol", ex);
			}
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
				throw new Exception("Error al obtener los roles y permisos por FK_IdPermiso", ex);
			}
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
				throw new Exception("Error al obtener los roles y permisos por FK_IdSistema", ex);
			}
			return View(nameof(DetallesRolesPermisos), rolPermisosList);
		}



		#endregion




		#region Métodos tipo INSERT, UPDATE y DELETE


		//											CREAR
		[HttpGet]
		public IActionResult CrearRolPermisos()
		{
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
				ModelState.AddModelError(string.Empty, "Error al crear el rol y permiso: " + ex.Message);
				return View(rolPermisos);
            }
		
			
		}



		//											ACTUALIZAR
		[HttpGet]
		public async Task<IActionResult> ActualizarRolPermisos(int IdRolPermisos)
		{
			var rolPermisos = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);

			if (rolPermisos == null)
			{
                TempData["ErrorMessage"] = "Rol y permiso no encontrado.";
				return RedirectToAction(nameof(DetallesRolesPermisos));
            }

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
				ModelState.AddModelError(string.Empty, "Error al actualizar el rol y permiso: " + ex.Message);
				return View(rolPermisos);
            }
			
		}



		//										    ELIMINAR
		[HttpGet]
		public async Task<IActionResult> EliminarRolPermisos(int IdRolPermisos)
		{
			var rolPermisos = await _daoRolesPermisos.ObtenerRolPermisosPorIdRolPermisosAsync(IdRolPermisos);

			if (rolPermisos == null)
			{
                TempData["ErrorMessage"] = "Rol y permiso no encontrado.";
				return RedirectToAction(nameof(DetallesRolesPermisos));
            }

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
				ModelState.AddModelError(string.Empty, "Error al eliminar el rol y permiso: " + ex.Message);
				return View(rolPermisos);
            }
			
			
		}



		#endregion
	}
}