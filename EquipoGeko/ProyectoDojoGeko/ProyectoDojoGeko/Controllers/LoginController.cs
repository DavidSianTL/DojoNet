using System.ComponentModel;
using System.Linq;
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
            string _connectionString = "Server=DARLA\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

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

                    // Vamos a traer el rol del usuario para verificar si está activo
                    var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(usuarioValido.IdUsuario);

                    // Verificamos si el resultado no es nulo
                    if (rolesUsuario is null)
                    {
                        // Si no se encuentra el rol, mostramos un mensaje de error
                        ViewBag.Mensaje = "Usuario no tiene rol asignado o no está activo.";
                        return RedirectToAction("Index", "Login");
                    }

                    // Si el método devuelve un objeto individual, lo usamos directamente
                    var rolUsuario = rolesUsuario;
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
                    var tokenModelDb = new ProyectoDojoGeko.Models.Usuario.TokenUsuarioViewModel
                    {
                        FK_IdUsuario = tokenModel.FK_IdUsuario,
                        Token = tokenModel.Token,
                        FechaCreacion = tokenModel.FechaCreacion,
                        TiempoExpira = tokenModel.TiempoExpira
                    };
                    _daoTokenUsuario.GuardarToken(tokenModelDb);

                    // Guardamos el token y el nombre de usuario en la sesión
                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetString("Usuario", usuarioValido.Username);
                    HttpContext.Session.SetString("Rol", nombreRol);

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
                // CÓDIGO TEMPORAL PARA TESTING
                if (usuario == "AdminDev" && password == "12345678")
                {
                    var jwtHelper = new JwtHelper();
                    var tokenModel = jwtHelper.GenerarToken(1, "AdminDev", 1, "Administrador");

                    _daoTokenUsuario.GuardarToken(tokenModel);

                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetString("Usuario", "AdminDev");
                    HttpContext.Session.SetString("Rol", "Administrador");

                    var hash = BCrypt.Net.BCrypt.HashPassword(password);
                    _daoTokenUsuario.GuardarContrasenia(1, hash);

                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Login Prueba",
                        Descripcion = $"Inicio de sesión de prueba exitoso para el usuario {usuario}.",
                        FK_IdUsuario = 1,
                        FK_IdSistema = 1
                    });

                    // ✅ CAMBIO: Redirigimos al Dashboard en lugar de Home
                    return RedirectToAction("Dashboard", "Dashboard");
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

        // NUEVAS ACCIONES PARA CAMBIO DE CONTRASEÑA
        // Acción GET para mostrar la vista de cambio de contraseña
        [HttpGet]
        public IActionResult CambioContraseña()
        {
            try
            {
                // Verificar que el usuario esté autenticado (opcional para presentación)
                var usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    // Para presentación, permitir acceso sin sesión
                    ViewBag.Usuario = "Usuario Demo";
                }
                else
                {
                    ViewBag.Usuario = usuario;
                }

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = "Error al cargar la página: " + e.Message;
                return View();
            }
        }

        // Acción POST para procesar el cambio de contraseña
        [HttpPost]
        public IActionResult CambioContraseña(string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            try
            {
                // Obtener usuario de la sesión
                var usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    // Para presentación, usar usuario demo
                    ViewBag.Usuario = "Usuario Demo";
                }
                else
                {
                    ViewBag.Usuario = usuario;
                }

                // Validaciones básicas
                if (string.IsNullOrEmpty(contraseñaActual) || string.IsNullOrEmpty(nuevaContraseña) || string.IsNullOrEmpty(confirmarContraseña))
                {
                    ViewBag.Mensaje = "Todos los campos son obligatorios.";
                    return View();
                }

                if (nuevaContraseña != confirmarContraseña)
                {
                    ViewBag.Mensaje = "Las contraseñas no coinciden.";
                    return View();
                }

                if (nuevaContraseña.Length < 8)
                {
                    ViewBag.Mensaje = "La nueva contraseña debe tener al menos 8 caracteres.";
                    return View();
                }

                // Para presentación: simular validación exitosa
                if (!string.IsNullOrEmpty(usuario) && usuario != "Usuario Demo")
                {
                    // Verificar la contraseña actual usando el método existente
                    var usuarioValido = _daoTokenUsuario.ValidarUsuario(usuario, contraseñaActual);

                    if (usuarioValido == null)
                    {
                        ViewBag.Mensaje = "La contraseña actual es incorrecta.";
                        return View();
                    }
                }

                // Simular cambio exitoso
                ViewBag.Mensaje = "¡Contraseña cambiada exitosamente! (Modo presentación)";

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Mensaje = "Error al procesar la solicitud: " + e.Message;
                ViewBag.Usuario = HttpContext.Session.GetString("Usuario") ?? "Usuario Demo";
                return View();
            }
        }

        // Método para cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); // Se ajustó el nombre de la acción de destino a "Index"
        }
    }
}
