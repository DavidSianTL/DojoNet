using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class UsuariosController : Controller
    {
        // Instanciamos el DAO
        private readonly daoUsuarioWSAsync _daoUsuarioWS;

        // Instanciamos el DAO de empleados
        private readonly daoEmpleadoWSAsync _daoEmpleado;

        // Instanciamos el DAO de bitácora
        private readonly daoBitacoraWSAsync _daoBitacora;

        // Instanciamos el DAO de log
        private readonly daoLogWSAsync _daoLog;

        // Instanciamos el servicio de envío de correos
        private readonly EmailService _emailService;

        // Constructor para inicializar la cadena de conexión
        public UsuariosController(EmailService emailService)
        {
            // Cadena de conexión a la DB de producción
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            // Cadena de conexión a la base de datos local
            // string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos el DAO con la cadena de conexión
            _daoUsuarioWS = new daoUsuarioWSAsync(_connectionString);
            // Inicializamos el DAO de empleados con la misma cadena de conexión
            _daoEmpleado = new daoEmpleadoWSAsync(_connectionString);
            // Inicializamos el DAO de bitácora con la misma cadena de conexión
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            // Inicializamos el DAO de log con la misma cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
            // Inicializamos el servicio de envío de correos
            _emailService = emailService;
        }

        // Acción que muestra la vista de usuarios
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor","Visualizador")]
        // [ValidateAntiForgeryToken]
        // El "ValidateAntiForgeryToken, solo se usa en peticiones Http de tipo POST, PUT y DELETE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtenemos los datos necesarios
                var usuarios = await _daoUsuarioWS.ObtenerUsuariosAsync();
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                // Preparamos el modelo para la vista
                var model = usuarios?.Select(u => new UsuarioFormViewModel
                {
                    // Asignamos el usuario
                    Usuario = u,
                    // Asignamos la lista de empleados para el dropdown
                    Empleados = empleados?.Select(e => new SelectListItem
                    {
                        // Asignamos el ID del empleado como valor
                        Value = e.IdEmpleado.ToString(),
                        // Asignamos el nombre y apellido del empleado como texto
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"

                        // De esta manera, si empleados es null, asignamos una lista vacía
                    }).ToList() ?? new List<SelectListItem>()

                    // De esta manera, si empleados es null, asignamos una lista vacía
                }).ToList() ?? new List<UsuarioFormViewModel>();

                // Registro en bitácora
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Insertamos en la bitácora el ingreso a la vista de usuarios
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Usuarios",
                    Descripcion = $"Ingreso a la vista de usuarios por {usuario}",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                // Devolvemos la vista con el modelo
                return View(model);

            }
            catch (Exception e)
            {

                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");

                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Vista Usuarios",
                    Descripcion = $"Error en el ingreso a la vista de usuarios del {usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View(new List<UsuarioFormViewModel>());
            }

        }

        // Acción para agregar un nuevo usuario
        // Solo SuperAdmin y Admin pueden ver la lista de usuarios
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            try
            {

                // Obtenemos el ID del usuario de la sesión
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                // Obtenemos el nombre de usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
                // Obtenemos el ID del sistema de la sesión
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                var model = new UsuarioFormViewModel
                {
                    Empleados = empleados.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                    }).ToList()
                };

                // Insertamos en la bítacora el ingreso a la vista de crear usuario
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Crear Usuario",
                    Descripcion = $"Ingreso a la vista de crear usuario, exitoso para el usuario {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(model);

            }
            catch (Exception e)
            {
                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");
                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Crear Usuario",
                    Descripcion = $"Error al ingresar a la vista de crear usuario del {usuario}: {e.Message}.",
                    Estado = false
                });
                // En caso de error, mostramos un mensaje genérico al usuario
                ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View();
            }
        }

        // Acción para crear un nuevo usuario
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioFormViewModel model)
        {
            try
            {
                // Validamos si el modelo es válido
                if (!ModelState.IsValid)
                {
                    // Extraemos el nombre de usuario de la sesión para registrar el error
                    var usuarioSesion = HttpContext.Session.GetString("Usuario");

                    // Registramos el error en los logs
                    await _daoLog.InsertarLogAsync(new LogViewModel
                    {
                        Accion = "Error Validación Usuario",
                        Descripcion = $"Intento de crear usuario con datos inválidos por {usuarioSesion}. Usuario: {model.Usuario.Username}, Empleado: {model.Usuario.FK_IdEmpleado}",
                        Estado = false
                    });

                    // Repoblar lista de empleados
                    var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                    model.Empleados = empleados.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                    }).ToList();

                    // Retornamos la vista con los errores de validación
                    ViewBag.Error = "Error en los datos para crear el usuario";
                    return View(model);
                }

                // Extraemos datos de la sesión para registrar eventos
                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuarioActual = HttpContext.Session.GetString("Usuario") ?? "Sistema";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Es una creación
                var (idUsuarioCreado, contraseniaGenerada) = await _daoUsuarioWS.InsertarUsuarioAsync(model.Usuario);

                // Obtener el empleado asociado al usuario
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(model.Usuario.FK_IdEmpleado);

                // Puedes usar el correo personal o institucional, según lo que necesites
                var emailDestino = empleado.CorreoInstitucional; // o empleado.CorreoPersonal

                // Creamos un destino "quemado" de momento
                //var emailDestino = "droblero@digitalgeko.com";

                // Creamos la ruta directamente
                var urlCambioPassword = Url.Action(
                    "CambioContrasena", // Acción
                    "Login",            // Controlador
                    new { id = model.Usuario.IdUsuario }, // Parámetros
                    protocol: Request.Scheme // "http" o "https"
                );

                // Enviamos el correo de bienvenida
                await _emailService.EnviarCorreoConMailjetAsync(usuarioActual,emailDestino, contraseniaGenerada, urlCambioPassword);

                // Registramos el evento de creación en la bitácora
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Crear Usuario",
                    Descripcion = $"Nuevo usuario '{model.Usuario.Username}' (Se ha creado un nuevo usuario ingresado por {usuarioActual}.",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = idSistema
                });

                // Después de crear el usuario, devolvemos al usuario a la vista de usuarios
                return RedirectToAction("Index");


            }
            catch (Exception e)
            {

                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuarioSesion = HttpContext.Session.GetString("Usuario");

                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Eliminar Usuario",
                    Descripcion = $"Error al intentar crear un nuevo usuario del {model.Usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index", "Usuarios");

            }

        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                // 1. Obtener el usuario a editar
                var usuario = await _daoUsuarioWS.ObtenerUsuarioPorIdAsync(id);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                // 2. Obtener la lista de empleados para el dropdown
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                // 3. Crear el ViewModel
                var model = new UsuarioFormViewModel
                {
                    Usuario = usuario,
                    Empleados = empleados.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception e)
            {
                var usuarioSession = HttpContext.Session.GetString("Usuario");
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Cargar Editar Usuario",
                    Descripcion = $"Error al cargar usuario ID {id} para edición por {usuarioSession}: {e.Message}",
                    Estado = false
                });

                TempData["Error"] = "Error al cargar el usuario para edición";
                return RedirectToAction("Index");
            }
        }


        // Acción para editar un usuario
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioViewModel usuario) // Cambiado a UsuarioViewModel
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si hay errores, recargamos los empleados y devolvemos la vista
                    var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                    var model = new UsuarioFormViewModel
                    {
                        Usuario = usuario,
                        Empleados = empleados.Select(e => new SelectListItem
                        {
                            Value = e.IdEmpleado.ToString(),
                            Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                        }).ToList()
                    };
                    return View(model);
                }

                await _daoUsuarioWS.ActualizarUsuarioAsync(usuario);

                // Registro en bitácora
                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Editar Usuario",
                    Descripcion = $"Usuario {usuario.Username} editado",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0
                });

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var usuarioSession = HttpContext.Session.GetString("Usuario");
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Editar Usuario",
                    Descripcion = $"Error al editar usuario ID {usuario.IdUsuario} por {usuarioSession}: {e.Message}",
                    Estado = false
                });

                TempData["Error"] = "Error al editar el usuario. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index");
            }
        }


        // Acción para "eliminar" un usuario (en realidad, cambiar su estado a inactivo)
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int Id)
        {
            try
            {
                // Validamos que el ID del usuario sea válido
                if (Id <= 0)
                {
                    ViewBag.Mensaje = "ID de usuario inválido.";
                    return View("Index", "Usuarios");
                }

                // Obtenemos el usuario a eliminar
                var usuario = await _daoUsuarioWS.ObtenerUsuarioPorIdAsync(Id);

                // Si el usuario no existe, mostramos un mensaje de error
                if (usuario == null)
                {
                    ViewBag.Mensaje = "Usuario no encontrado.";
                    return View("Index", "Usuarios");
                }

                // Extraemos el ID del usuario a eliminar
                var idUsuarioEncontrado = usuario.IdUsuario;

                // Actualizamos el usuario en la base de datos
                await _daoUsuarioWS.EliminarUsuarioAsync(idUsuarioEncontrado);

                // Extraemos el ID del usuario de la sesión para registrar el evento
                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

                // Extraemos el ID del sistema de la sesión para registrar el evento
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Registramos el evento en la bitácora
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Eliminar Usuario",
                    Descripcion = $"El usuario con ID {idUsuarioEncontrado} ha sido eliminado.",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = idSistema
                });

                // Redirigimos a la vista de usuarios con un mensaje de éxito
                ViewBag.Mensaje = "Usuario eliminado correctamente.";
                return View("Index", "Usuarios");

            }
            catch (Exception e)
            {

                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");

                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Eliminar Usuario",
                    Descripcion = $"Error al eliminar el usuario con ID {Id} del {usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index", "Usuarios");

            }


        }




    }
}