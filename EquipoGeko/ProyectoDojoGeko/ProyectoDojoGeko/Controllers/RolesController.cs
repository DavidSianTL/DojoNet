using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class RolesController : Controller
    {
        // Dependencias de acceso a datos
        private readonly daoRolesWSAsync _dao;
        // Dependencia para registrar logs
        private readonly daoLogWSAsync _daoLog;
        // Dependencia para registrar bitácoras
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        // Dependencia para manejar roles de usuario
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        // Constructor que inicializa las dependencias con la cadena de conexión
        public RolesController()
        {
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            _dao = new daoRolesWSAsync(connectionString);// Inicializa el acceso a datos de roles
            _daoLog = new daoLogWSAsync(connectionString);//    Inicializa el acceso a datos de logs
            _daoBitacoraWS = new daoBitacoraWSAsync(connectionString);// Inicializa el acceso a datos de bitácoras
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);// Inicializa el acceso a datos de roles de usuario
        }

        private async Task RegistrarLogYBitacora(string accion, string descripcion)
        {
            // Método para registrar un log y una bitácora de forma asíncrona
            try
            {
                // Inserta un log en la base de datos
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = accion,
                    Descripcion = descripcion,
                    Estado = true
                });

                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;// Obtiene el ID del usuario de la sesión actual
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);// Obtiene los roles del usuario actual
                var idSistema = rolesUsuario.FirstOrDefault()?.FK_IdSistema ?? 0;// Obtiene el ID del sistema asociado al usuario

                // Inserta una entrada en la bitácora
                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = accion,
                    Descripcion = descripcion,
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en bitácora/log: {ex}");
            }
        }

        // Método para mostrar la lista de roles
        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _dao.ObtenerRolesAsync();
                return View(roles);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Index Roles", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar el formulario de creación de un nuevo rol
        public IActionResult Crear() => View();

        [HttpPost]
        // Método para procesar la creación de un nuevo rol
        public async Task<IActionResult> Crear(RolesViewModel rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.InsertarRolAsync(rol);
                    await RegistrarLogYBitacora("Crear Rol", $"Rol '{rol.NombreRol}' creado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Crear Rol", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar los detalles de un rol específico
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var rol = await _dao.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Detalles Rol", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar los detalles de un rol específico
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var rol = await _dao.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Rol (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        // Método para procesar la edición de un rol existente
        public async Task<IActionResult> Editar(RolesViewModel rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.ActualizarRolAsync(rol);
                    await RegistrarLogYBitacora("Editar Rol", $"Rol '{rol.NombreRol}' actualizado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Rol (POST)", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar el formulario de eliminación de un rol
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var rol = await _dao.ObtenerRolPorIdAsync(id);
                if (rol == null)
                    return NotFound();

                return View(rol);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Rol (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Eliminar")]
        //
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.DesactivarRolAsync(id);
                await RegistrarLogYBitacora("Eliminar Rol", $"Rol con ID {id} desactivado.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Rol (POST)", ex.Message);
                return View("Error");
            }
        }
    }
}