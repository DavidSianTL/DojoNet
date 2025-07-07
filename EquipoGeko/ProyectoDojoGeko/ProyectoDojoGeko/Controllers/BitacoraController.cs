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
            string _connectionString = "Server=DESKTOP-44A3MP2;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            // string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicializamos el DAO de bitácoras con la misma cadena de conexión
            _daoBitacoraWS = new daoBitacoraWSAsync(_connectionString);
            // Inicializamos el DAO de roles con la cadena de conexión
            _daoRolUsuario = new daoUsuariosRolWSAsync(_connectionString);
            // Inicializamos el DAO de logs con la cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Acción que muestra la vista de bitácora
        // Solo SuperAdmin, Admin pueden acceder a esta vista
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor","Visualizador")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtenemos el ID del usuario de la sesión
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Verificamos si el usuario tiene el rol de SuperAdmin o Admin
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);

                // Verificamos si la lista no está vacía
                if (rolesUsuario is null)
                {
                    // Si no se encuentra el rol, mostramos un mensaje de error
                    ViewBag.ErrorMessage = "Usuario no tiene el rol correcto asignado o no está activo.";
                    return View(new List<BitacoraViewModel>());
                }

                // Obtenemos todas las bitácoras ORDENADAS POR FECHA DESCENDENTE (más recientes primero)
                var bitacoras = await _daoBitacoraWS.ObtenerBitacorasAsync();
                
                // Asegurar que estén ordenadas por fecha descendente
                if (bitacoras != null)
                {
                    bitacoras = bitacoras.OrderByDescending(b => b.FechaEntrada).ToList();
                }

                // Registramos la acción en la bitácora
                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = "Consulta Bitácora",
                    Descripcion = "El usuario ha consultado los registros de bitácora",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(bitacoras);
            }
            catch (Exception e)
            {
                // En caso de error, registramos el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error al Consultar Bitácora",
                    Descripcion = $"Error al consultar la bitácora: {e.Message}",
                    Estado = false
                });

                // Mostramos un mensaje de error
                ViewBag.ErrorMessage = "Ocurrió un error al cargar los registros de bitácora.";
                return View(new List<BitacoraViewModel>());
            }
        }

        // Acción para guardar una nueva bitácora - PERMISOS CORREGIDOS
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> GuardarBitacora(BitacoraViewModel bitacora)
        {
            try
            {
                // Obtenemos el ID del usuario de la sesión
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(bitacora.Accion))
                {
                    TempData["ErrorMessage"] = "La acción es requerida.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(bitacora.Descripcion))
                {
                    TempData["ErrorMessage"] = "La descripción es requerida.";
                    return RedirectToAction("Index");
                }

                if (bitacora.FK_IdUsuario <= 0)
                {
                    TempData["ErrorMessage"] = "El ID de usuario debe ser un número positivo.";
                    return RedirectToAction("Index");
                }

                if (bitacora.FK_IdSistema <= 0)
                {
                    TempData["ErrorMessage"] = "El ID de sistema debe ser un número positivo.";
                    return RedirectToAction("Index");
                }

                // VERIFICACIÓN DE PERMISOS SIMPLIFICADA
                // Solo verificamos que el usuario esté logueado y tenga una sesión válida
                if (idUsuario <= 0)
                {
                    TempData["ErrorMessage"] = "Sesión de usuario no válida. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("Index", "Login");
                }

                // Verificamos si el usuario tiene roles asignados
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);

                // Si el usuario tiene roles, verificamos que tenga permisos
                if (rolesUsuario != null && rolesUsuario.Any())
                {
                    // En caso de que hayan múltiples roles, traemos todos los nombres de los roles del usuario
                    var nombresRoles = rolesUsuario
                        .Where(ru => ru.Rol != null)
                        .Select(ru => ru.Rol.NombreRol)
                        .ToList();

                    // Verificamos si el usuario tiene al menos uno de los roles permitidos
                    if (!nombresRoles.Contains("SuperAdmin") && !nombresRoles.Contains("Admin") && !nombresRoles.Contains("Usuario"))
                    {
                        TempData["ErrorMessage"] = "Usuario no tiene permisos para realizar esta acción.";
                        return RedirectToAction("Index");
                    }
                }

                // Si no se especifica fecha, usar la fecha actual
                if (bitacora.FechaEntrada == default(DateTime))
                {
                    bitacora.FechaEntrada = DateTime.UtcNow;
                }

                // Insertamos el registro en la bitácora en la base de datos
                await _daoBitacoraWS.InsertarBitacoraAsync(bitacora);

                // Creamos una bitácora del evento de inserción
                var bitacoraEvento = new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = "Insertar Bitácora Manual",
                    Descripcion = $"Se ha insertado manualmente una nueva bitácora con acción: {bitacora.Accion}",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                };

                // Insertamos la bitácora del evento en la base de datos
                await _daoBitacoraWS.InsertarBitacoraAsync(bitacoraEvento);

                // Mensaje de éxito
                TempData["SuccessMessage"] = "Registro de bitácora guardado exitosamente.";

                // Redirigir al índice para mostrar los registros actualizados
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                // En caso de error, registramos el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error al Guardar Bitácora Manual",
                    Descripcion = $"Error al guardar bitácora manual: {e.Message}. Datos recibidos: Acción={bitacora?.Accion}, Descripción={bitacora?.Descripcion}, Usuario={bitacora?.FK_IdUsuario}, Sistema={bitacora?.FK_IdSistema}",
                    Estado = false
                });

                // Redirigimos a la vista de índice con un mensaje de error detallado
                TempData["ErrorMessage"] = $"Ocurrió un error al guardar el registro: {e.Message}. Por favor, verifique los datos e inténtelo de nuevo.";
                return RedirectToAction("Index");
            }
        }

        // Acción para obtener bitácoras por AJAX (opcional para filtros avanzados)
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador")]
        public async Task<IActionResult> ObtenerBitacoras(int? idUsuario = null, string accion = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                // Obtenemos todas las bitácoras ordenadas por fecha descendente
                var bitacoras = await _daoBitacoraWS.ObtenerBitacorasAsync();
                
                if (bitacoras != null)
                {
                    bitacoras = bitacoras.OrderByDescending(b => b.FechaEntrada).ToList();

                    // Aplicamos filtros si se proporcionan
                    if (idUsuario.HasValue)
                    {
                        bitacoras = bitacoras.Where(b => b.FK_IdUsuario == idUsuario.Value).ToList();
                    }

                    if (!string.IsNullOrEmpty(accion))
                    {
                        bitacoras = bitacoras.Where(b => b.Accion.Contains(accion, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    if (fechaDesde.HasValue)
                    {
                        bitacoras = bitacoras.Where(b => b.FechaEntrada >= fechaDesde.Value).ToList();
                    }

                    if (fechaHasta.HasValue)
                    {
                        bitacoras = bitacoras.Where(b => b.FechaEntrada <= fechaHasta.Value).ToList();
                    }
                }

                return Json(bitacoras);
            }
            catch (Exception e)
            {
                // En caso de error, registramos el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error al Obtener Bitácoras",
                    Descripcion = $"Error al obtener bitácoras: {e.Message}",
                    Estado = false
                });

                return Json(new { error = "Error al obtener los registros de bitácora" });
            }
        }

        // Acción para exportar bitácoras a Excel (opcional)
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador")]
        public async Task<IActionResult> ExportarExcel()
        {
            try
            {
                // Obtenemos todas las bitácoras ordenadas por fecha descendente
                var bitacoras = await _daoBitacoraWS.ObtenerBitacorasAsync();
                
                if (bitacoras != null)
                {
                    bitacoras = bitacoras.OrderByDescending(b => b.FechaEntrada).ToList();
                }

                // Registramos la acción en la bitácora
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = "Exportar Bitácora Excel",
                    Descripcion = "El usuario ha exportado los registros de bitácora a Excel",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                // Aquí implementarías la lógica para generar el archivo Excel
                // Por ahora, redirigimos de vuelta al índice
                TempData["SuccessMessage"] = "Exportación a Excel iniciada. El archivo se descargará automáticamente.";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                // En caso de error, registramos el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error al Exportar Bitácora Excel",
                    Descripcion = $"Error al exportar bitácora a Excel: {e.Message}",
                    Estado = false
                });

                TempData["ErrorMessage"] = "Error al exportar los registros de bitácora a Excel.";
                return RedirectToAction("Index");
            }
        }

        // Acción para exportar bitácoras a PDF (opcional)
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador")]
        public async Task<IActionResult> ExportarPDF()
        {
            try
            {
                // Obtenemos todas las bitácoras ordenadas por fecha descendente
                var bitacoras = await _daoBitacoraWS.ObtenerBitacorasAsync();
                
                if (bitacoras != null)
                {
                    bitacoras = bitacoras.OrderByDescending(b => b.FechaEntrada).ToList();
                }

                // Registramos la acción en la bitácora
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    FechaEntrada = DateTime.UtcNow,
                    Accion = "Exportar Bitácora PDF",
                    Descripcion = "El usuario ha exportado los registros de bitácora a PDF",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                // Aquí implementarías la lógica para generar el archivo PDF
                // Por ahora, redirigimos de vuelta al índice
                TempData["SuccessMessage"] = "Exportación a PDF iniciada. El archivo se descargará automáticamente.";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                // En caso de error, registramos el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error al Exportar Bitácora PDF",
                    Descripcion = $"Error al exportar bitácora a PDF: {e.Message}",
                    Estado = false
                });

                TempData["ErrorMessage"] = "Error al exportar los registros de bitácora a PDF.";
                return RedirectToAction("Index");
            }
        }
    }
}
