using Microsoft.AspNetCore.Mvc;
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

        // Instanciamos el DAO de bitácora
        private readonly daoBitacoraWSAsync _daoBitacora;

        // Instanciamos el DAO de log
        private readonly daoLogWSAsync _daoLog;

        // Constructor para inicializar la cadena de conexión
        public UsuariosController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoUsuarioWS = new daoUsuarioWSAsync(_connectionString);
            // Inicializamos el DAO de bitácora con la misma cadena de conexión
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            // Inicializamos el DAO de log con la misma cadena de conexión
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Acción que muestra la vista de usuarios
        [AuthorizeRole("SuperAdmin", "Admin")]
        // [ValidateAntiForgeryToken]
        // El "ValidateAntiForgeryToken, solo se usa en peticiones Http de tipo POST, PUT y DELETE
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtenemos la lista de usuarios desde el DAO
                var usuarios = await _daoUsuarioWS.ObtenerUsuariosAsync();

                // Si la lista de usuarios no es nula y tiene elementos, la devolvemos a la vista
                if (usuarios != null && usuarios.Any())
                    return View(usuarios);


                // Guardamos el token y el nombre de usuario en la sesión
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Insertamos en la bítacora el inicio de sesión exitoso
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Usuarios",
                    Descripcion = $"Ingreso a la vista de usuarios, exitoso para el usuario {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                // Redirigimos y le pasamos la lista de usuarios a la vista
                return View(new List<UsuarioViewModel>());

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
                return View(new List<UsuarioViewModel>());
            }

        }

        // Acción para agregar un nuevo usuario
        // Solo SuperAdmin y Admin pueden ver la lista de usuarios
        [AuthorizeRole("SuperAdmin", "Admin")]
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

                // Insertamos en la bítacora el ingreso a la vista de crear usuario
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Crear Usuario",
                    Descripcion = $"Ingreso a la vista de crear usuario, exitoso para el usuario {usuario}.",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View();

            }
            catch(Exception e)
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

        // Añadir la acción POST para CrearEditar que maneja tanto creación como edición
        // Esta acción recibe un UsuarioViewModel y guarda los datos en la base de datos
        // Solo SuperAdmin y Admin pueden crear o editar usuarios
        // Si el IdUsuario es mayor que 0, se trata de una actualización; si es 0, es una creación
        /*[AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CrearEditar(UsuarioViewModel usuario)
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
                        Descripcion = $"Intento de {(usuario.IdUsuario > 0 ? "actualizar" : "crear")} usuario con datos inválidos por {usuarioSesion}. Usuario: {usuario.Username}",
                        Estado = false
                    });

                    // Retornamos la vista con los errores de validación
                    ViewBag.Error = usuario.IdUsuario > 0
                        ? "Error en los datos para actualizar el usuario"
                        : "Error en los datos para crear el usuario";
                    return View(usuario);
                }

                // Extraemos datos de la sesión para registrar eventos
                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuarioActual = HttpContext.Session.GetString("Usuario") ?? "Sistema";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Verificamos si el usuario ya existe en la base de datos
                if (usuario.IdUsuario > 0)
                {
                    // Es una actualización
                    await _daoUsuarioWS.ActualizarUsuarioAsync(usuario);

                    // Registramos el evento de actualización en la bitácora
                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Actualizar Usuario",
                        Descripcion = $"El usuario '{usuario.Username}' (ID: {usuario.IdUsuario}) ha sido actualizado por {usuarioActual}.",
                        FK_IdUsuario = idUsuarioSesion,
                        FK_IdSistema = idSistema
                    });

                    // Registramos el evento exitoso en los logs
                    await _daoLog.InsertarLogAsync(new LogViewModel
                    {
                        Accion = "Actualizar Usuario",
                        Descripcion = $"Usuario '{usuario.Username}' actualizado correctamente por {usuarioActual}.",
                        Estado = true
                    });

                    TempData["SuccessMessage"] = "Usuario actualizado correctamente";
                }
                else
                {
                    // Es una creación
                    var usuarioCreado = await _daoUsuarioWS.InsertarUsuarioAsync(usuario);

                    // Si el método InsertarUsuarioAsync retorna el usuario con ID, usamos ese ID
                    // Si no, usamos el ID del modelo (que debería ser asignado)
                    var idUsuarioCreado = usuarioCreado > 0 ? usuarioCreado : usuario.IdUsuario;

                    // Registramos el evento de creación en la bitácora
                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Crear Usuario",
                        Descripcion = $"Nuevo usuario '{usuario.Username}' (ID: {idUsuarioCreado}) ha sido creado por {usuarioActual}.",
                        FK_IdUsuario = idUsuarioSesion,
                        FK_IdSistema = idSistema
                    });

                    // Registramos el evento exitoso en los logs
                    await _daoLog.InsertarLogAsync(new LogViewModel
                    {
                        Accion = "Crear Usuario",
                        Descripcion = $"Usuario '{usuario.Username}' creado correctamente por {usuarioActual}.",
                        Estado = true
                    });

                    TempData["SuccessMessage"] = "Usuario creado correctamente";
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                // Extraemos el nombre de usuario de la sesión para el manejo de errores
                var usuarioSesion = HttpContext.Session.GetString("Usuario") ?? "Sistema";

                // Registramos el error en los logs
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = usuario.IdUsuario > 0 ? "Error Actualizar Usuario" : "Error Crear Usuario",
                    Descripcion = $"Error al {(usuario.IdUsuario > 0 ? "actualizar" : "crear")} usuario '{usuario.Username}' por {usuarioSesion}: {e.Message}",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje genérico al usuario
                Console.WriteLine($"Error al guardar usuario: {e.Message}");
                ViewBag.Error = usuario.IdUsuario > 0
                    ? "Error al actualizar el usuario. Por favor, inténtelo de nuevo."
                    : "Error al crear el usuario. Por favor, inténtelo de nuevo.";

                return View(usuario);
            }
        }*/

        // Acción para crear un nuevo usuario
        [AuthorizeRole("SuperAdmin", "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioViewModel usuario)
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
                        Descripcion = $"Intento de crear usuario con datos inválidos por {usuarioSesion}. Usuario: {usuario.Username}",
                        Estado = false
                    });

                    // Retornamos la vista con los errores de validación
                    ViewBag.Error = "Error en los datos para crear el usuario";
                    return View(usuario);
                }

                // Extraemos datos de la sesión para registrar eventos
                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuarioActual = HttpContext.Session.GetString("Usuario") ?? "Sistema";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                // Es una creación
                var usuarioCreado = await _daoUsuarioWS.InsertarUsuarioAsync(usuario);

                // Registramos el evento de creación en la bitácora
                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Crear Usuario",
                    Descripcion = $"Nuevo usuario '{usuario.Username}' (Se ha creado un nuevo usuario ingresado por {usuarioActual}.",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = idSistema
                });

                // Después de crear el usuario, devolvemos al usuario a la vista de usuarios
                return RedirectToAction("Index");


            }
            catch(Exception e)
            {

                // En caso de error, volvemos a extraer el nombre de usuario de la sesión
                var usuarioSesion = HttpContext.Session.GetString("Usuario");

                // Insertamos en el log el error del proceso de login
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Eliminar Usuario",
                    Descripcion = $"Error al intentar crear un nuevo usuario del {usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index", "Usuarios");

            }

        }


        // Acción para "eliminar" un usuario (en realidad, cambiar su estado a inactivo)
        [AuthorizeRole("SuperAdmin", "Admin")]
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
