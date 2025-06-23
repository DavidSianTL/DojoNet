using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Helper;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Controllers
{
    public class LoginController : Controller
    {
        // Instanciamos el DAO de tokens
        private readonly daoTokenUsuario _daoTokenUsuario;

        // Instanciamos el DAO de logs
        private readonly daoLogWSAsync _daoLog;

        // Instanciamos el DAO de bítacoras
        private readonly daoBitacoraWSAsync _daoBitacora;

        // Instanciamos el DAO de usuarios
        private readonly daoUsuarioWSAsync _daoUsuario;

        // Instanciamos el DAO de empleados
        private readonly daoEmpleadoWSAsync _daoEmpleado;

        // Instanciamos el DAO de correo
        private readonly EmailService _emailService;

        // Instanciamos el DAO de rol para validar el rol del usuario
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        // Insanciamos el DAO de roles
        private readonly daoRolesWSAsync _daoRol;

        // Instanciamos el DAO de roles y permisos
        private readonly daoRolPermisosWSAsync _daoRolPermisos;

        // Constructor para inicializar la cadena de conexión
        public LoginController(EmailService emailService)
        {
            // Cadena de conexión a la base de datos - ACTUALIZADA
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            //string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos el DAO de tokens con la misma cadena de conexión
            _daoTokenUsuario = new daoTokenUsuario(_connectionString);

            // Inicializamos el DAO de logs 
            _daoLog = new daoLogWSAsync(_connectionString);

            // Inicializamos el DAO de bítacoras
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);

            // Inicializamos el DAO de usuarios
            _daoUsuario = new daoUsuarioWSAsync(_connectionString);

            // Inicializamos el DAO de empleados
            _daoEmpleado = new daoEmpleadoWSAsync(_connectionString);

            // Inicializamos el DAO de correo
            _emailService = emailService;

            // Inicializamos el DAO de roles de usuario
            _daoRolUsuario = new daoUsuariosRolWSAsync(_connectionString);

            // Inicializamos el DAO de usuarios rol
            _daoRol = new daoRolesWSAsync(_connectionString);

            // Inicializamos el DAO de roles y permisos
            _daoRolPermisos = new daoRolPermisosWSAsync(_connectionString);
        }

        // Acción que muestra la vista de inicio de sesión
        [HttpGet]
        public IActionResult Index() // Se cambió el nombre de Login() a Index() para que coincida con la vista Index.cshtml
        {
            return View(); // Muestra la vista Views/Login/Index.cshtml
        }

        // Acción que muestra la vista de inicio de sesión
        [HttpGet]
        public IActionResult IndexCambioContrasenia() // Se cambió el nombre de Login() a Index() para que coincida con la vista Index.cshtml
        {
            return View(); // Muestra la vista Views/Login/Index.cshtml
        }

        // Acción que maneja el inicio de sesión
        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string password)
        {
            try
            {
                // Validamos el usuario y la clave usando el DAO de tokens
                var usuarioValido = _daoTokenUsuario.ValidarUsuario(usuario, password);

                // Si el usuario es válido, generamos un token JWT y lo guardamos
                if (usuarioValido != null)
                {
                    // Verificamos si el usuario está activo
                    var jwtHelper = new JwtHelper();

                    // Vamos a trear el rol del usuario para verificar si está activo
                    var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(usuarioValido.IdUsuario);

                    // Verificamos si la lista no está vacía
                    if (rolesUsuario is null)
                    {
                        // Si no se encuentra el rol, mostramos un mensaje de error
                        ViewBag.Mensaje = "Usuario no tiene rol asignado o no está activo.";
                        return RedirectToAction("Index", "Login");
                    }

                    // Obtenemos el primer rol del usuario
                    var rolUsuario = rolesUsuario.FirstOrDefault();
                    var idRol = rolUsuario.FK_IdRol;

                    // Obtenemos el ID del sistema por medio del ID del rol
                    var sistemaRol = await _daoRolPermisos.ObtenerRolPermisosPorIdRolAsync(idRol);

                    // Verificamos si el sistemaRol es nulo
                    if (sistemaRol is null)
                    {
                        // Si no se encuentra el sistema, mostramos un mensaje de error
                        ViewBag.Mensaje = "El sistema asociado a este rol, no existe";
                        return RedirectToAction("Index", "Login");
                    }

                    // Obtenemos el ID del sistema
                    var sis = sistemaRol.FirstOrDefault();

                    // Asignamos el ID del sistema
                    var idSistema = sis.FK_IdSistema;

                    // Obtenemos el nombre del rol
                    var roles = await _daoRol.ObtenerRolPorIdAsync(idRol);

                    // Verificamos si la lista no está vacía
                    if (roles is null)
                    {
                        // Si no se encuentra el rol, mostramos un mensaje de error
                        ViewBag.Mensaje = "El Rol no existe";
                        return RedirectToAction("Index", "Login");
                    }

                    // Obtenemos el nombre del rol
                    var nombreRol = roles.NombreRol;

                    // Generamos el token JWT para el usuario
                    var tokenModel = jwtHelper.GenerarToken(usuarioValido.IdUsuario, usuarioValido.Username, idRol, nombreRol);

                    // Guardamos el token en la base de datos
                    _daoTokenUsuario.GuardarToken(tokenModel);

                    // Guardamos el token y el nombre de usuario en la sesión
                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetInt32("IdUsuario", usuarioValido.IdUsuario);
                    HttpContext.Session.SetString("Usuario", usuarioValido.Username);
                    HttpContext.Session.SetString("Rol", nombreRol);
                    HttpContext.Session.SetInt32("IdSistema", idSistema);

                    // Insertamos en la bítacora el inicio de sesión exitoso
                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Login",
                        Descripcion = $"Inicio de sesión exitoso para el usuario {usuarioValido.Username}.",
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        FK_IdSistema = idSistema
                    });

                    // Redirigimos a la acción a Dashboard
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    // Si el usuario no es válido, mostramos un mensaje de error
                    ViewBag.Mensaje = "Usuario o clave incorrectos.";
                    // Retornamos la vista de inicio de sesión con el mensaje de error
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception e)
            {
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Login",
                    Descripcion = $"Error en el proceso de login para usuario {usuario}: {e.Message}.",
                    Estado = false
                });

                // En caso de error, mostramos un mensaje de error genérico al usuario
                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index", "Login");
            }
        }

        // Acción para pruebas de inicio de sesión (para presentación)
        [HttpPost]
        public async Task<IActionResult> LoginPrueba(string usuario, string password)
        {
            try
            {

                usuario = "AdminDev";

                // CÓDIGO TEMPORAL PARA TESTING
                if (usuario == "AdminDev" && password == "12345678")
                {
                    var jwtHelper = new JwtHelper();

                    // Simulamos datos reales para el usuario AdminDev
                    int idUsuario = 1;
                    int idRol = 4;
                    int idSistema = 10;
                    string rol = "SuperAdministrador";

                    // Generamos el token JWT
                    var tokenModel = jwtHelper.GenerarToken(idUsuario, usuario, idRol, rol);

                    if (tokenModel != null)
                    {
                        // Revocamos cualquier token anterior (incluso si no existe)
                        _daoTokenUsuario.RevocarToken(idUsuario);

                        // Guardamos el nuevo token en la BD
                        _daoTokenUsuario.GuardarToken(new TokenUsuarioViewModel
                        {
                            FK_IdUsuario = idUsuario,
                            Token = tokenModel.Token,
                            FechaCreacion = tokenModel.FechaCreacion,
                            TiempoExpira = tokenModel.TiempoExpira
                        });

                        // Guardamos los datos en sesión
                        HttpContext.Session.SetString("Token", tokenModel.Token);
                        HttpContext.Session.SetInt32("IdUsuario", idUsuario);
                        HttpContext.Session.SetString("Usuario", usuario);
                        HttpContext.Session.SetString("Rol", rol);
                        HttpContext.Session.SetInt32("IdSistema", idSistema);

                        // Hasheamos y guardamos la contraseña en la BD (solo para pruebas)
                        var hash = BCrypt.Net.BCrypt.HashPassword(password);
                        _daoTokenUsuario.GuardarContrasenia(idUsuario, hash);

                        // Insertamos en bitácora
                        await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                        {
                            Accion = "Login Prueba",
                            Descripcion = $"Inicio de sesión de prueba exitoso para el usuario {usuario}.",
                            FK_IdUsuario = idUsuario,
                            FK_IdSistema = idSistema
                        });

                        // Redirigimos al Dashboard
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo generar el token.";
                        return View("Index");
                    }
                }
                else
                {
                    ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
                    return View("Index");
                }
            }
            catch (Exception e)
            {
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Login",
                    Descripcion = $"Error en el proceso de login para usuario {usuario}: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index");
            }
        }

        // Acción para pruebas de inicio de sesión (para presentación)
        [HttpPost]
        public async Task<IActionResult> LoginCambioContrasenia(string usuario, string password)
        {
            try
            {
                // Validamos el usuario y la clave usando el DAO de tokens
                var usuarioValido = _daoTokenUsuario.ValidarUsuarioCambioContrasenia(usuario, password);

                if (usuarioValido == null)
                {
                    ViewBag.Mensaje = "Usuario o clave incorrectos.";
                    return View("IndexCambioContrasenia");
                }

                var jwtHelper = new JwtHelper();

                // Vamos a trear el rol del usuario para verificar si está activo
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(usuarioValido.IdUsuario);

                // Verificamos si la lista no está vacía
                if (rolesUsuario == null)
                {
                    // Si no se encuentra el rol, mostramos un mensaje de error
                    ViewBag.Mensaje = "Usuario no tiene rol asignado o no está activo.";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                // Obtenemos el primer rol del usuario
                var rolUsuario = rolesUsuario.FirstOrDefault();

                if (rolesUsuario == null || !rolesUsuario.Any())
                {
                    ViewBag.Mensaje = "Usuario no tiene rol asignado o no está activo.";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                // Obtenemos el ID del rol
                var idRol = rolUsuario.FK_IdRol;

                // Obtenemos el nombre del rol
                var roles = await _daoRol.ObtenerRolPorIdAsync(idRol);

                // Verificamos si la lista no está vacía
                if (roles == null)
                {
                    // Si no se encuentra el rol, mostramos un mensaje de error
                    ViewBag.Mensaje = "El Rol no existe";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                // Obtenemos el ID del sistema por medio del ID del rol
                var sistemaRol = await _daoRolPermisos.ObtenerRolPermisosPorIdRolAsync(idRol);

                // Verificamos si el sistemaRol es nulo
                if (sistemaRol == null)
                {
                    // Si no se encuentra el sistema, mostramos un mensaje de error
                    ViewBag.Mensaje = "El sistema asociado a este rol, no existe";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                // Obtenemos el primer sistema del rol
                var sis = sistemaRol.FirstOrDefault();

                // Asignamos el ID del sistema
                var idSistema = sis.FK_IdSistema;

                // Obtenemos el nombre del rol
                var rol = roles.NombreRol;

                // Generamos el token JWT
                var tokenModel = jwtHelper.GenerarToken(usuarioValido.IdUsuario, usuarioValido.Username, idRol, rol);

                if (tokenModel != null)
                {

                    // Guardamos el nuevo token en la BD
                    _daoTokenUsuario.GuardarToken(new TokenUsuarioViewModel
                    {
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        Token = tokenModel.Token,
                        FechaCreacion = tokenModel.FechaCreacion,
                        TiempoExpira = tokenModel.TiempoExpira
                    });

                    // Guardamos los datos en sesión
                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetInt32("IdUsuario", usuarioValido.IdUsuario);
                    HttpContext.Session.SetString("Usuario", usuario);
                    HttpContext.Session.SetString("Rol", rol);
                    HttpContext.Session.SetInt32("IdSistema", idSistema);

                    // Insertamos en bitácora
                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Login Prueba",
                        Descripcion = $"Inicio de sesión de prueba exitoso para el usuario {usuario}.",
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        FK_IdSistema = idSistema
                    });

                    // Redirigimos al Cambio de contrasenia
                    return RedirectToAction(nameof(CambioContrasena));
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo generar el token.";
                    return RedirectToAction(nameof(LoginCambioContrasenia));
                }

            }
            catch (Exception e)
            {
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error Login",
                    Descripcion = $"Error en el proceso de login para usuario {usuario}: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SolicitarNuevaContrasenia(string username)
        {
            // Buscar usuario por username
            var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
            if (usuarios == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado" });
            }
            
            // Buscar usuario por username
            var usuario = usuarios.FirstOrDefault(u => u.Username == username);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado" });
            }

            // Buscamos el correo del usuario
            var empleado = await _daoEmpleado.ObtenerEmpleadoAsync();

            // Buscamos el correo del usuario
            var correo = empleado.FirstOrDefault(e => e.IdEmpleado == usuario.FK_IdEmpleado);
            if(correo == null)
            {
                return Json(new { success = false, message = "No se han encontrado correos asociados al usuario" });
            }
            
            // Generar nueva contraseña y hash
            string nuevaContrasenia = daoUsuarioWSAsync.GenerarContraseniaAleatoria();
            string hash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasenia);
            DateTime nuevaExpiracion = DateTime.UtcNow.AddHours(1);

            // Actualizar en la BD (nuevo método que actualice contraseña y expiración)
            await _daoUsuario.ActualizarContraseniaExpiracionAsync(usuario.IdUsuario, hash, nuevaExpiracion);

            // URL de cambio de contraseña
            string urlCambioPassword = "https://localhost:7001/CambioContrasena";

            // Enviar correo con la nueva contraseña
            await _emailService.EnviarCorreoConMailjetAsync(usuario.Username, correo.CorreoInstitucional, nuevaContrasenia, urlCambioPassword);

            return Json(new { success = true, message = "Se ha enviado una nueva contraseña a tu correo." });
        }

        // ACCIONES PARA CAMBIO DE CONTRASEÑA - ACTUALIZADAS
        // Acción GET para mostrar la vista de cambio de contraseña
        [HttpGet]
        public IActionResult CambioContrasena()
        {
            try
            {
                // Verificar que el usuario esté autenticado
                /*var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                var usuario = HttpContext.Session.GetString("Usuario");

                if (idUsuario == null || string.IsNullOrEmpty(usuario))
                {
                    // Si no hay sesión activa, redirigir al login
                    return RedirectToAction("Index", "Login");
                }

                ViewBag.Usuario = usuario;*/
                return View();
            }
            catch (Exception e)
            {
                // Registrar el error en el log
                _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error CambioContrasena GET",
                    Descripcion = $"Error al cargar la página de cambio de contraseña: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al cargar la página. Por favor, inténtelo de nuevo.";
                return View();
            }
        }

        // Acción POST para procesar el cambio de contraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambioContrasena(string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            try
            {
                // Obtener datos del usuario de la sesión
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema");

                if (idUsuario == null || string.IsNullOrEmpty(usuario))
                {
                    // Si no hay sesión activa, redirigir al login
                    return RedirectToAction("Index", "Login");
                }

                ViewBag.Usuario = usuario;

                // Validaciones básicas
                if (string.IsNullOrEmpty(contraseñaActual) || string.IsNullOrEmpty(nuevaContraseña) || string.IsNullOrEmpty(confirmarContraseña))
                {
                    ViewBag.Mensaje = "Todos los campos son obligatorios.";
                    return View();
                }

                if (nuevaContraseña != confirmarContraseña)
                {
                    ViewBag.Mensaje = "Las contraseñas nuevas no coinciden.";
                    return View();
                }

                // Validaciones de seguridad para la nueva contraseña
                if (nuevaContraseña.Length < 8)
                {
                    ViewBag.Mensaje = "La nueva contraseña debe tener al menos 8 caracteres.";
                    return View();
                }

                // Verificar si contiene al menos una letra mayúscula, una minúscula y un número
                bool tieneMayuscula = nuevaContraseña.Any(char.IsUpper);
                bool tieneMinuscula = nuevaContraseña.Any(char.IsLower);
                bool tieneNumero = nuevaContraseña.Any(char.IsDigit);

                if (!tieneMayuscula || !tieneMinuscula || !tieneNumero)
                {
                    ViewBag.Mensaje = "La contraseña debe contener al menos una letra mayúscula, una minúscula y un número.";
                    return View();
                }

                // Verificar que la contraseña actual sea correcta
                var usuarioValido = _daoTokenUsuario.ValidarUsuarioCambioContrasenia(usuario, contraseñaActual);

                if (usuarioValido == null)
                {
                    ViewBag.Mensaje = "La contraseña actual es incorrecta.";
                    return View();
                }

                // Verificar que la nueva contraseña sea diferente a la actual
                if (contraseñaActual == nuevaContraseña)
                {
                    ViewBag.Mensaje = "La nueva contraseña debe ser diferente a la actual.";
                    return View();
                }

                // Generar hash de la nueva contraseña
                string hashNuevaContraseña = BCrypt.Net.BCrypt.HashPassword(nuevaContraseña);

                // Actualizar la contraseña en la base de datos
                // Como GuardarContrasenia es void, usamos try-catch para manejar el resultado
                try
                {
                    _daoTokenUsuario.GuardarContrasenia(usuarioValido.IdUsuario, hashNuevaContraseña);

                    // Si llegamos aquí, la operación fue exitosa
                    // Registrar el cambio en la bitácora
                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Cambio de Contraseña",
                        Descripcion = $"El usuario {usuario} ha cambiado su contraseña exitosamente.",
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        FK_IdSistema = idSistema ?? 1
                    });

                    // Revocar tokens anteriores para forzar un nuevo inicio de sesión
                    _daoTokenUsuario.RevocarToken(usuarioValido.IdUsuario);

                    // Limpiar la sesión
                    HttpContext.Session.Clear();

                    // Mostrar mensaje de éxito y redirigir al login
                    TempData["MensajeExito"] = "Contraseña cambiada exitosamente. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    // Error específico al guardar la contraseña
                    await _daoLog.InsertarLogAsync(new LogViewModel
                    {
                        Accion = "Error GuardarContrasenia",
                        Descripcion = $"Error al guardar la nueva contraseña para el usuario {usuario}: {ex.Message}",
                        Estado = false
                    });

                    ViewBag.Mensaje = "Error al actualizar la contraseña. Por favor, inténtelo de nuevo.";
                    return View();
                }
            }
            catch (Exception e)
            {
                // Registrar el error en el log
                await _daoLog.InsertarLogAsync(new LogViewModel
                {
                    Accion = "Error CambioContrasena POST",
                    Descripcion = $"Error al procesar el cambio de contraseña: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View();
            }
        }

        // Método para cerrar sesión
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); // Se ajustó el nombre de la acción de destino a "Index"
        }
    }
}