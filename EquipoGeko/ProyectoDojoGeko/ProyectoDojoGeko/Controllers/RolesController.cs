using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
<<<<<<< HEAD
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class RolesController : Controller
    {
        // Dependencias de acceso a datos
=======
    [AuthorizeSession] 
    [AuthorizeRole("SuperAdmin")] // Solo SuperAdmin puede administrar roles
    public class RolesController : Controller
    {
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
        private readonly daoRolesWSAsync _dao;
        // Dependencia para registrar logs
        private readonly daoLogWSAsync _daoLog;
        // Dependencia para registrar bitácoras
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        // Dependencia para manejar roles de usuario
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

<<<<<<< HEAD
        // Constructor que inicializa las dependencias con la cadena de conexión
        public RolesController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
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
=======
        // Constructor que inicializa la conexión con la base de datos
        public RolesController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            _dao = new daoRolesWSAsync(connectionString);
        }

        // Mostrar lista de roles
        public async Task<IActionResult> Index()
        {
            var roles = await _dao.ObtenerRolesAsync();
            return View(roles);
        }

        // Mostrar formulario para crear rol
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // Guardar nuevo rol
        [HttpPost]
        public async Task<IActionResult> Crear(RolesViewModel rol)
        {
            if (ModelState.IsValid)
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
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
<<<<<<< HEAD
            catch (Exception ex)
=======
            return View(rol);
        }

        // Mostrar detalles de un rol
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Mostrar formulario para editar rol
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Guardar cambios del rol
        [HttpPost]
        public async Task<IActionResult> Editar(RolesViewModel rol)
        {
            if (ModelState.IsValid)
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
            {
                await RegistrarLogYBitacora("Error Eliminar Rol (POST)", ex.Message);
                return View("Error");
            }
<<<<<<< HEAD
=======
            return View(rol);
        }

        // Mostrar vista de confirmación para eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rol = await _dao.ObtenerRolPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // Confirmar cambiar estado del rol a inactivo.
        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            // Cambiar el estado del rol a inactivo en lugar de eliminarlo físicamente
            await _dao.DesactivarRolAsync(id); 
            return RedirectToAction(nameof(Index));
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
        }
    }
}
