using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models.Usuario;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
	public class UsuarioRolController : Controller
	{
		private daoUsuariosRolWSAsync _daoUsuariosRol;

		public UsuarioRolController()
		{
			// Cadena de conexión a la base de datos
			string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
			// Inicializamos el DAO con la cadena de conexión
			_daoUsuariosRol = new daoUsuariosRolWSAsync(_connectionString);
		}


		


		#region Métodos de obtención de datos

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolAsync();

			return View(usuariosRolList);
		}



		[HttpGet]
        public async Task<IActionResult> UsuariosRolPorId()
		{
			return View();
		}

        [HttpPost]
		public async Task<IActionResult> UsuariosRolPorId(int Id) 
		{
			UsuariosRolViewModel usuariosRol = new UsuariosRolViewModel();

			try
			{
                usuariosRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(Id);

			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el usuario y rol por ID", ex);
			}

			return View(usuariosRol);
		}




		[HttpGet]
		public async Task<IActionResult> UsuariosRolPorIdRol(int Id)
		{
			var usuariosRol = new UsuariosRolViewModel();

			try
			{
				usuariosRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdRolAsync(Id);

			}
			catch
			{
				throw new Exception("Error al obtener el usuario y rol por ID de rol");
			}

			return View(usuariosRol);
		}

		[HttpGet]
		public async Task<IActionResult> UsuariosRolPorIdUsuario(int Id)
		{
			var usuariosRol = new UsuariosRolViewModel();
			try
			{
				usuariosRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdUsuarioAsync(Id);
			}
			catch
			{
				throw new Exception("Error al obtener el usuario y rol por ID de usuario");
			}
			return View(usuariosRol);
		}

		#endregion



		#region Metodos de inserción y actualización


		[HttpGet]
		public IActionResult InsertarUsuarioRol()
		{
			return View();
		}
			

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertarUsuarioRol(UsuariosRolViewModel usuarioRol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool resultado = await _daoUsuariosRol.InsertarUsuarioRolAsync(usuarioRol);
                    if (resultado)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo insertar el UsuarioRol.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al insertar el UsuarioRol: {ex.Message}");
                }
            }
            return View(usuarioRol);
        }



		[HttpGet]
		public async Task<IActionResult> EditarUsuarioRol(int id)
		{
			var usuarioRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(id);
			if (usuarioRol == null)
			{
				return NotFound();
			}
			return View(usuarioRol);
        }








        #endregion




    }
}
