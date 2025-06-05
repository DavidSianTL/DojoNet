using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class BitacoraController : Controller
    {

        // Instanciamos el DAO de bitácoras para registrar eventos
        private readonly daoBitacoraWSAsync _daoBitacoraWS;

        // Instanciamos el DAO de bitácoras para registrar eventos
        private readonly daoLogWSAsync _daoLog;

        // Instanciamos el DAO de rol para validar el rol del usuario
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        // Constructor para inicializar la cadena de conexión
        public BitacoraController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicializamos el DAO de bitácoras con la misma cadena de conexión
            _daoBitacoraWS = new daoBitacoraWSAsync(_connectionString);
            // Inicializamos el DAO de roles con la cadena de conexión
            _daoRolUsuario = new daoUsuariosRolWSAsync(_connectionString);
            // Inicializamos el DAO de logs con la cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Acción que muestra la vista de usuarios
        // Solo SuperAdmin, Admin pueden acceder a esta vista
        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        // Acción para guardar una nueva bitácora
        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> GuardarBitacora(BitacoraViewModel bitacora)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    // Obtenemos el ID del usuario de la sesión
                    // Asignamos un valor por defecto de 0 si no hay ID en la sesión
                    var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

                    // Verificamos si el usuario tiene el rol de SuperAdmin o Admin
                    var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);

                    // Verificamos si la lista no está vacía
                    if (rolesUsuario is null)
                    {
                        // Si no se encuentra el rol, mostramos un mensaje de error
                        ViewBag.Mensaje = "Usuario no tiene el rol correcto asignado o no está activo.";
                        return RedirectToAction("Index", "Login");
                    }

                    // En caso de que hayan múltiples roles, traemos todos los nombres de los roles del usuario
                    var nombresRoles = rolesUsuario
                        .Where(ru => ru.Rol != null)
                        .Select(ru => ru.Rol.NombreRol)
                        .ToList();

                    // Verificamos si el usuario tiene el rol de SuperAdmin o Admin
                    if (!nombresRoles.Contains("SuperAdmin") && !nombresRoles.Contains("Admin"))
                    {
                        // Si el usuario no tiene el rol de SuperAdmin o Admin, mostramos un mensaje de error
                        ViewBag.Mensaje = "Usuario no tiene permisos para realizar esta acción.";
                        return RedirectToAction("Index", "Login");
                    }

                    // Insertamos el registro en la bitácora en la base de datos
                    await _daoBitacoraWS.InsertarBitacoraAsync(bitacora);

                    // Obtenemos el primer rol del usuario
                    var rolUsuario = rolesUsuario.FirstOrDefault();

                    // Extramos el ID del sistema del rol del usuario
                    var idSistema = rolUsuario.FK_IdRol;

                    // Creamos una bitácora del evento
                    var bitacoras = new BitacoraViewModel
                    {
                        FechaEntrada = DateTime.UtcNow,
                        Accion = "Insertar Bitácora",
                        Descripcion = $"Se ha insertado una nueva bitácora con ID: {bitacora.IdBitacora}",
                        FK_IdUsuario = idUsuario, // Aquí deberías obtener el ID del usuario actual
                        FK_IdSistema = idSistema // Aquí deberías obtener el ID del sistema actual
                    };

                    // Insertamos la bitácora en la base de datos
                    await _daoBitacoraWS.InsertarBitacoraAsync(bitacoras);

                    // Registramos el evento en el log
                    return RedirectToAction(nameof(Index));
                }
                return View(bitacora);
            }
            catch (Exception e)
            {
                // En caso de error, registramos el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error al Guardar manualmente un Log",
                    Descripcion = $"Error al guardar el log: {e.Message}",
                    Estado = false
                });

                // Redirigimos a la vista de índice con un mensaje de error
                ViewBag.ErrorMessage = "Ocurrió un error al guardar el log. Por favor, inténtelo de nuevo.";
                // Usamos nameof para redirigir a la acción Index
                return RedirectToAction(nameof(Index));
            }

        }


    }
}