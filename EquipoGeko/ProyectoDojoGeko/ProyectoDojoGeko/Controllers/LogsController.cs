using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class LogsController : Controller
    {
        // Instanciamos el DAO de logs para registrar eventos
        private readonly daoLogWSAsync _daoLog;

        // Instanciamos el DAO de bitácoras para registrar eventos
        private readonly daoBitacoraWSAsync _daoBitacoraWS;

        // Instanciamos el DAO de rol para validar el rol del usuario
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        // Constructor para inicializar la cadena de conexión
        public LogsController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicializamos el DAO de roles con la cadena de conexión
            _daoRolUsuario = new daoUsuariosRolWSAsync(_connectionString);
            // Inicializamos el DAO de logs con la cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
            // Inicializamos el DAO de bitácoras con la misma cadena de conexión
            _daoBitacoraWS = new daoBitacoraWSAsync(_connectionString);
        }

        // Acción que muestra la vista de usuarios
        // Solo SuperAdmin, Admin pueden acceder a esta vista
        [HttpGet]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        // Acción para guardar un nuevo log
        [HttpPost]
        [AuthorizeRole("SuperAdmin", "Admin")]
        public async Task<IActionResult> GuardarLog(LogViewModel log)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Insertamos el log en la base de datos
                    await _daoLog.InsertarLogAsync(log);

                    // Obtenemos el ID del usuario de la sesión
                    // Asignamos un valor por defecto de 0 si no hay ID en la sesión
                    var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

                    // Verificamos si el usuario tiene el rol de SuperAdmin o Admin
                    var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);

                    // Verificamos si la lista no está vacía
                    if (rolesUsuario is null)
                    {
                        // Si no se encuentra el rol, mostramos un mensaje de error
                        ViewBag.Mensaje = "Usuario no tiene rol asignado o no está activo.";
                        return RedirectToAction("Index", "Login");
                    }

                    // Obtenemos el primer rol del usuario
                    var rolUsuario = rolesUsuario.FirstOrDefault();

                    // Extramos el ID del sistema del rol del usuario
                    var idSistema = rolUsuario.FK_IdSistema;


                    // Creamos una bitácora del evento
                    var bitacora = new BitacoraViewModel
                    {
                        FechaEntrada = DateTime.UtcNow,
                        Accion = "Insertar Log",
                        Descripcion = $"Se ha insertado un nuevo log con ID: {log.IdLog}",
                        FK_IdUsuario = idUsuario, // Aquí deberías obtener el ID del usuario actual
                        FK_IdSistema = idSistema // Aquí deberías obtener el ID del sistema actual
                    };

                    // Insertamos la bitácora en la base de datos
                    await _daoBitacoraWS.InsertarBitacoraAsync(bitacora);

                    return RedirectToAction(nameof(Index));
                }
                return View(log);
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