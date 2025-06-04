using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models.Usuario;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Controllers
{
    public class UsuariosRolController : Controller
    {
        private daoUsuariosRolWSAsync _daoUsuariosRol;

        public UsuariosRolController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoUsuariosRol = new daoUsuariosRolWSAsync(_connectionString);
        }



        #region Métodos de obtención de datos

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> Index()
        {
            var usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolAsync();

            return View(usuariosRolList);
        }


        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public IActionResult DetalleRolUsuario(UsuariosRolViewModel usuarioRol)
        {
            return View(usuarioRol);
        }


        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> UsuarioRolPorId(int Id)
        {
            List<UsuariosRolViewModel> usuariosRolList = new List<UsuariosRolViewModel>();

            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(Id);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuarioRol por ID", ex);
            }

            return View(nameof(DetalleRolUsuario), usuariosRolList);
        }




        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> UsuarioRolPorIdRol(int Id)
        {
            List<UsuariosRolViewModel> usuariosRolList = new List<UsuariosRolViewModel>();

            try
            {
                usuariosRolList = await _daoUsuariosRol.ObtenerUsuariosRolPorIdRolAsync(Id);

            }
            catch
            {
                throw new Exception("Error al obtener el usuario y rol por ID de rol");
            }

            return View(nameof(DetalleRolUsuario), usuariosRolList);
        }

        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> UsuarioRolPorIdUsuario(int Id)
        {
            List<UsuariosRolViewModel> usuariosRolList = new List<UsuariosRolViewModel>();
            try
            {
                usuariosRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdUsuarioAsync(Id);
            }
            catch
            {
                throw new Exception("Error al obtener el usuario y rol por ID de usuario");
            }
            return View(nameof(DetalleRolUsuario), usuariosRol);
        }

        #endregion



        #region Metodos de inserción y actualización


        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public IActionResult InsertarUsuarioRol()
        {
            return View();
        }


        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
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
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> EditarUsuarioRol(int id)
        {
            var usuarioRol = await _daoUsuariosRol.ObtenerUsuariosRolPorIdAsync(id);
            if (usuarioRol == null)
            {
                return NotFound();
            }
            return View(usuarioRol);
        }

        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> EditarUsuarioRol(UsuariosRolViewModel usuarioRol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool resultado = await _daoUsuariosRol.ActualizarUsuarioRolAsync(usuarioRol);
                    if (resultado)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo actualizar el UsuarioRol.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al actualizar el UsuarioRol: {ex.Message}");
                }

            }

            return View(usuarioRol);

        }



        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> EliminarUsuarioRol(int IdUsrioRol)
        {
            try
            {
                bool resultado = await _daoUsuariosRol.EliminarUsuarioRolAsync(IdUsrioRol);
                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo eliminar el UsuarioRol.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al eliminar el UsuarioRol: {ex.Message}");
            }
            return View(IdUsrioRol);
        }

        #endregion

    }
}