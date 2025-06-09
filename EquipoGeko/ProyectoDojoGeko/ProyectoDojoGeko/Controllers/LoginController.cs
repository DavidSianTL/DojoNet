using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
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

        // Instanciamos el DAO de rol para validar el rol del usuario
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;

        // Insanciamos el DAO de roles
        private readonly daoRolesWSAsync _daoRol;

        // Constructor para inicializar la cadena de conexión
        public LoginController()
        {
            // Cadena de conexión a la base de datos - ACTUALIZADA
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            // string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos el DAO de tokens con la misma cadena de conexión
            _daoTokenUsuario = new daoTokenUsuario(_connectionString);

            // Inicializamos el DAO de logs 
            _daoLog = new daoLogWSAsync(_connectionString);

            // Inicializamos el DAO de bítacoras
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);

            // Inicializamos el DAO de roles de usuario
            _daoRolUsuario = new daoUsuariosRolWSAsync(_connectionString);

            // Inicializamos el DAO de usuarios rol
            _daoRol = new daoRolesWSAsync(_connectionString);
        }

        // Acción que muestra la vista de inicio de sesión
        [HttpGet]
        public IActionResult Index() // Se cambió el nombre de Login() a Index() para que coincida con la vista Index.cshtml
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
                    var idSistema = rolUsuario.FK_IdSistema;

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

                usuario = "Prueba1";

                // CÓDIGO TEMPORAL PARA TESTING
                if (usuario == "Prueba1" && password == "12345678")
                {
                    var jwtHelper = new JwtHelper();

                    // Simulamos datos reales para el usuario AdminDev
                    int idUsuario = 4;
                    int idSistema = 1;
                    string rol = "SuperAdmin";

                    // Generamos el token JWT
                    var tokenModel = jwtHelper.GenerarToken(idUsuario, usuario, idSistema, rol);

                    if (tokenModel != null)
                    {
                        // Revocamos cualquier token anterior
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

        // ACCIONES PARA CAMBIO DE CONTRASEÑA - ACTUALIZADAS
        // Acción GET para mostrar la vista de cambio de contraseña
        [HttpGet]
        public IActionResult CambioContrasena()
        {
            try
            {
                // Verificar que el usuario esté autenticado
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                var usuario = HttpContext.Session.GetString("Usuario");

                if (idUsuario == null || string.IsNullOrEmpty(usuario))
                {
                    // Si no hay sesión activa, redirigir al login
                    return RedirectToAction("Index", "Login");
                }

                ViewBag.Usuario = usuario;
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
                var usuarioValido = _daoTokenUsuario.ValidarUsuario(usuario, contraseñaActual);

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