using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
<<<<<<< HEAD
using ProyectoDojoGeko.Filters;
=======
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1

namespace ProyectoDojoGeko.Controllers
    
{
<<<<<<< HEAD
    [AuthorizeSession]
    [AuthorizeRole("SuperAdmin")]
    public class PermisosController : Controller
    {
        // Dependencias de acceso a datos
        private readonly daoPermisosWSAsync _dao;// Acceso a datos de permisos
        private readonly daoLogWSAsync _daoLog;//   Acceso a datos de logs
        private readonly daoBitacoraWSAsync _daoBitacoraWS;// Acceso a datos de bitácoras
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;// Acceso a datos de roles de usuario
=======
    //Requiere de la sesión de usuario autorizada
    [AuthorizeSession] 
    public class PermisosController : Controller
    {
        private readonly daoPermisosWSAsync _dao;
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1

        public PermisosController()
        {
            string connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
<<<<<<< HEAD
            _dao = new daoPermisosWSAsync(connectionString);// Inicializa el acceso a datos de permisos
            _daoLog = new daoLogWSAsync(connectionString);//    Inicializa el acceso a datos de logs
            _daoBitacoraWS = new daoBitacoraWSAsync(connectionString);// Inicializa el acceso a datos de bitácoras
            _daoRolUsuario = new daoUsuariosRolWSAsync(connectionString);// Inicializa el acceso a datos de roles de usuario
        }

        // Método para registrar un log y una bitácora de forma asíncrona
        private async Task RegistrarLogYBitacora(string accion, string descripcion)
        {
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

        public async Task<IActionResult> Index()
        {
            try
=======
            _dao = new daoPermisosWSAsync(connectionString);
        }

        // Mostrar lista de permisos
        public async Task<IActionResult> Index()
        {
            var permisos = await _dao.ObtenerPermisosAsync();
            return View(permisos);
        }

        // Ver detalles de un permiso
        [AuthorizeRole("SuperAdmin")]
        public async Task<IActionResult> LISTAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Formulario de creación de permiso
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public IActionResult CREAR()
        {
            return View();
        }

        // Crear permiso (POST)
        [AuthorizeRole("SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CREAR(PermisoViewModel permiso)
        {
            if (ModelState.IsValid)
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
            {
                var permisos = await _dao.ObtenerPermisosAsync();
                return View(permisos);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Index Permisos", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar el formulario de creación de permisos
        public IActionResult Crear() => View();

        [HttpPost]
        // Método para procesar la creación de un nuevo permiso
        public async Task<IActionResult> Crear(PermisoViewModel permiso)
        {
            try
            {
                // Verifica si el modelo es válido antes de insertar el permiso
                if (ModelState.IsValid)
                {
                    await _dao.InsertarPermisoAsync(permiso);
                    await RegistrarLogYBitacora("Crear Permiso", $"Permiso '{permiso.NombrePermiso}' creado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Crear Permiso", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar los detalles de un permiso específico
        public async Task<IActionResult> Detalles(int id)
        {
            // Intenta obtener los detalles del permiso por su ID
            try
            {
                var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Detalles Permiso", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar el formulario de edición de un permiso
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Permiso (GET)", ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        // Método para procesar la edición de un permiso
        public async Task<IActionResult> Editar(PermisoViewModel permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dao.ActualizarPermisoAsync(permiso);
                    await RegistrarLogYBitacora("Editar Permiso", $"Permiso '{permiso.NombrePermiso}' actualizado.");
                    return RedirectToAction(nameof(Index));
                }
                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Editar Permiso (POST)", ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        // Método para mostrar el formulario de eliminación de un permiso
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
                if (permiso == null)
                    return NotFound();

                return View(permiso);
            }
            catch (Exception ex)
            {
                await RegistrarLogYBitacora("Error Eliminar Permiso (GET)", ex.Message);
                return View("Error");
            }
        }
        // Método para procesar la eliminación de un permiso
        [HttpPost, ActionName("Eliminar")]
        // EliminarConfirmado
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            try
            {
                await _dao.EliminarPermisoAsync(id);
                await RegistrarLogYBitacora("Eliminar Permiso", $"Permiso con ID {id} desactivado.");
                return RedirectToAction(nameof(Index));
            }
<<<<<<< HEAD
            catch (Exception ex)
=======
            return View(permiso);
        }

        // Formulario para editar un permiso
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> EDITAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Editar permiso (POST)
        [AuthorizeRole("SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> EDITAR(PermisoViewModel permiso)
        {
            if (ModelState.IsValid)
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
            {
                await RegistrarLogYBitacora("Error Eliminar Permiso (POST)", ex.Message);
                return View("Error");
            }
<<<<<<< HEAD
=======
            return View(permiso);
        }

        // Confirmación para eliminar permiso
        [AuthorizeRole("SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> ELIMINAR(int id)
        {
            var permiso = await _dao.ObtenerPermisoPorIdAsync(id);
            if (permiso == null)
                return NotFound();

            return View(permiso);
        }

        // Eliminar permiso (POST)
        [AuthorizeRole("SuperAdmin")]
        [HttpPost, ActionName("ELIMINAR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dao.EliminarPermisoAsync(id);
            return RedirectToAction(nameof(Index));
>>>>>>> fb32e12858b4a67d5bc32477c1b83fa800a6dfe1
        }
    }
}
