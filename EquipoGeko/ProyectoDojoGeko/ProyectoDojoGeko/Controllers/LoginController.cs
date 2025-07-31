using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Dtos.Login.Requests;
using ProyectoDojoGeko.Helper;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    public class LoginController : Controller
    {
        private readonly daoTokenUsuario _daoTokenUsuario;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoEmpleadosEmpresaDepartamentoWSAsync _daoEmpleadoEmpresaDepartamento;
        private readonly EmailService _emailService;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;
        private readonly daoRolesWSAsync _daoRol;
        private readonly daoRolPermisosWSAsync _daoRolPermisos;
        private readonly ILoggingService _loggingService;

        public LoginController(
            EmailService emailService,
            daoTokenUsuario daoTokenUsuario,
            daoBitacoraWSAsync daoBitacora,
            daoUsuarioWSAsync daoUsuario,
            daoEmpleadoWSAsync daoEmpleado,
            daoEmpleadosEmpresaDepartamentoWSAsync daoEmpleadoEmpresaDepartamento,
            daoUsuariosRolWSAsync daoRolUsuario,
            daoRolesWSAsync daoRol,
            daoRolPermisosWSAsync daoRolPermisos,
            ILoggingService loggingService)
        {
            _emailService = emailService;
            _daoTokenUsuario = daoTokenUsuario;
            _daoBitacora = daoBitacora;
            _daoUsuario = daoUsuario;
            _daoEmpleado = daoEmpleado;
            _daoEmpleadoEmpresaDepartamento = daoEmpleadoEmpresaDepartamento;
            _daoRolUsuario = daoRolUsuario;
            _daoRol = daoRol;
            _daoRolPermisos = daoRolPermisos;
            _loggingService = loggingService;
        }
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult IndexCambioContrasenia() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensaje = "Datos de inicio de sesión inválidos";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                // Validamos el usuario y la clave usando el DAO de tokens
                var usuarioValido = _daoTokenUsuario.ValidarUsuario(request.Usuario, request.Password);

                // Si el usuario es válido, generamos un token JWT y lo guardamos
                if (usuarioValido != null)
                {
                    // Obtenemos la información completa del usuario para verificar su estado
                    var usuarioCompleto = await _daoUsuario.ObtenerUsuarioPorIdAsync(usuarioValido.IdUsuario);

                    // Verificamos si el usuario está activo (FK_IdEstado = 1)
                    if (usuarioCompleto.FK_IdEstado != 1)
                    {
                        // Si el estado no es 'Activo', mostramos un mensaje adecuado
                        ViewBag.Mensaje = "Tu cuenta está pendiente de aprobación o ha sido desactivada.";
                        return RedirectToAction("Index", "Login");
                    }

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

                    // Obtenemos todos los roles del usuario
                    var rolesDelUsuario = new List<string>();
                    var idSistema = 0;
                    var nombreRol = "";

                    // Procesamos cada rol del usuario
                    foreach (var rolUsuario in rolesUsuario)
                    {
                        var idRol = rolUsuario.FK_IdRol;

                        /*
                            // Obtenemos el sistema por el ID del rol
                            var sistemaRol = await _daoRolPermisos.ObtenerRolPermisosPorIdRolAsync(idRol);

                            // Verificamos si el sistemaRol es nulo
                            if (sistemaRol is null || !sistemaRol.Any())
                            {
                                continue; // Saltamos este rol si no tiene sistema asociado
                            }

                            // Obtenemos el ID del sistema (usamos el primer sistema del rol)
                            var sis = sistemaRol.FirstOrDefault();
                            idSistema = sis.FK_IdSistema; // Guardamos el ID del sistema
                        */

                        // Obtenemos el nombre del rol
                        var rol = await _daoRol.ObtenerRolPorIdAsync(idRol);
                        if (rol != null)
                        {
                            rolesDelUsuario.Add(rol.NombreRol);
                            // Guardamos el primer rol como rol principal (para compatibilidad)
                            if (string.IsNullOrEmpty(nombreRol))
                            {
                                nombreRol = rol.NombreRol;
                            }

                        }
                    }

                    // Verificamos si el usuario tiene al menos un rol válido
                    if (rolesDelUsuario.Count == 0)
                    {
                        // Si no se encontraron roles válidos, mostramos un mensaje de error
                        ViewBag.Mensaje = "El usuario no tiene roles asignados o no están activos.";
                        return RedirectToAction("Index", "Login");
                    }

                    // Generamos el token JWT para el usuario
                    var tokenModel = jwtHelper.GenerarToken(usuarioValido.IdUsuario, usuarioValido.Username, rolesUsuario.FirstOrDefault().FK_IdRol, nombreRol);

                    // Guardamos el token en la base de datos
                    _daoTokenUsuario.GuardarToken(tokenModel);

                    // Obtenemos los datos del empleado asociado al usuario
                    var empleados = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(usuarioValido.FK_IdEmpleado);

                    // Obtenemos la relación entre el empleado y la empresa
                    var empleadoEmpresa = await _daoEmpleadoEmpresaDepartamento.ObtenerEmpleadoEmpresaPorIdAsync(empleados.IdEmpleado);

                    // Obtenemos el nombre completo del empleado
                    var nombreCompletoEmpleado = $"{empleados.NombresEmpleado} {empleados.ApellidosEmpleado}";

                    // Guardamos el tipo de contrato, código del empleado y el nombre completo en la sesión
                    HttpContext.Session.SetString("TipoContrato", empleados.TipoContrato);
                    HttpContext.Session.SetInt32("IdEmpleado", empleados.IdEmpleado);
                    HttpContext.Session.SetString("CodigoEmpleado", empleados.CodigoEmpleado);
                    HttpContext.Session.SetString("NombreCompletoEmpleado", nombreCompletoEmpleado);

                    // Guardamos el token y la información del usuario en la sesión
                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetInt32("IdUsuario", usuarioValido.IdUsuario);
                    HttpContext.Session.SetString("Usuario", usuarioValido.Username);

                    // Guardamos el rol principal (para compatibilidad)
                    HttpContext.Session.SetString("Rol", nombreRol);

                    // Guardamos todos los roles como una lista separada por comas
                    HttpContext.Session.SetString("Roles", string.Join(",", rolesDelUsuario));

                    // Guardamos el ID del sistema
                    HttpContext.Session.SetInt32("IdSistema", idSistema);

                    // Guardamos el ID de la empresa
                    HttpContext.Session.SetInt32("IdEmpresa", empleadoEmpresa.FK_IdEmpresa);

                    // Insertamos en la bítacora el inicio de sesión exitoso
                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Login",
                        Descripcion = $"Inicio de sesión exitoso para el usuario {usuarioValido.Username}.",
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        FK_IdSistema = idSistema
                    });

                    // Redirigimos a la acción a Dashboard
                    return RedirectToAction("Dashboard", "Dashboard"); // En caso de éxito
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
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Login",
                    Descripcion = $"Error en el proceso de login para usuario {request.Usuario}: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginPrueba(string usuario, string password)
        {
            try
            {
                usuario = "AdminDev";

                if (usuario == "AdminDev" && password == "12345678")
                {
                    var jwtHelper = new JwtHelper();
                    int idUsuario = 1;
                    int idRol = 4;
                    int idSistema = 0;
                    int idEmpresa = 1; // Asignamos un ID de empresa para la prueba
                    string rolX = "Empleado";
                    string rolY = "TeamLider";

                    List<string> roles = new List<string> { rolX, rolY};

                    var tokenModel = jwtHelper.GenerarToken(idUsuario, usuario, idRol, rolX);

                    if (tokenModel != null)
                    {
                        _daoTokenUsuario.RevocarToken(idUsuario);

                        _daoTokenUsuario.GuardarToken(new TokenUsuarioViewModel
                        {
                            FK_IdUsuario = idUsuario,
                            Token = tokenModel.Token,
                            FechaCreacion = tokenModel.FechaCreacion,
                            TiempoExpira = tokenModel.TiempoExpira
                        });

                        // Obtenemos los datos del empleado asociado al usuario
                        var empleados = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(1);

                        // Vemos la relación entre el empleado y la empresa
                        // var empleadoEmpresa = await _daoEmpleadoEmpresaDepartamento.ObtenerEmpleadoEmpresaPorIdAsync(idEmpresa);

                        // Obtenemos el nombre completo del empleado
                        var nombreCompletoEmpleado = $"{empleados.NombresEmpleado} {empleados.ApellidosEmpleado}";

                        // Guardamos el tipo de contrato, código del empleado y el nombre completo en la sesión
                        HttpContext.Session.SetString("TipoContrato", empleados.TipoContrato);
                        HttpContext.Session.SetInt32("IdEmpleado", empleados.IdEmpleado);
                        HttpContext.Session.SetString("CodigoEmpleado", empleados.CodigoEmpleado);
                        HttpContext.Session.SetString("NombreCompletoEmpleado", nombreCompletoEmpleado);

                        HttpContext.Session.SetString("Token", tokenModel.Token);
                        HttpContext.Session.SetInt32("IdUsuario", idUsuario);
                        HttpContext.Session.SetString("Usuario", usuario);
                        HttpContext.Session.SetString("Rol", rolX);
                        HttpContext.Session.SetString("Roles", string.Join(",", roles));
                        HttpContext.Session.SetInt32("IdSistema", idSistema);
                        HttpContext.Session.SetInt32("IdEmpresa", idEmpresa);

                        var hash = BCrypt.Net.BCrypt.HashPassword(password);
                        _daoTokenUsuario.GuardarContrasenia(idUsuario, hash);

                        await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                        {
                            Accion = "Login Prueba",
                            Descripcion = $"Inicio de sesión de prueba exitoso para el usuario {usuario}.",
                            FK_IdUsuario = idUsuario,
                            FK_IdSistema = idSistema
                        });

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
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error LoginPrueba",
                    Descripcion = $"Error en login de prueba para usuario {usuario}: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginCambioContrasenia(string usuario, string password)
        {
            try
            {
                var usuarioValido = _daoTokenUsuario.ValidarUsuarioCambioContrasenia(usuario, password);

                if (usuarioValido == null)
                {
                    ViewBag.Mensaje = "Usuario o clave incorrectos.";
                    return View("IndexCambioContrasenia");
                }

                var jwtHelper = new JwtHelper();
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(usuarioValido.IdUsuario);

                if (rolesUsuario == null || !rolesUsuario.Any())
                {
                    ViewBag.Mensaje = "Usuario no tiene rol asignado o no está activo.";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                var rolUsuario = rolesUsuario.First();
                var idRol = rolUsuario.FK_IdRol;
                var roles = await _daoRol.ObtenerRolPorIdAsync(idRol);

                if (roles == null)
                {
                    ViewBag.Mensaje = "El Rol no existe.";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                var sistemaRol = await _daoRolPermisos.ObtenerRolPermisosPorIdRolAsync(idRol);

                if (sistemaRol == null || !sistemaRol.Any())
                {
                    ViewBag.Mensaje = "El sistema asociado a este rol no existe.";
                    return RedirectToAction("IndexCambioContrasenia", "Login");
                }

                var idSistema = sistemaRol.First().FK_IdSistema;
                var rol = roles.NombreRol;

                var tokenModel = jwtHelper.GenerarToken(usuarioValido.IdUsuario, usuarioValido.Username, idRol, rol);

                if (tokenModel != null)
                {
                    _daoTokenUsuario.GuardarToken(new TokenUsuarioViewModel
                    {
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        Token = tokenModel.Token,
                        FechaCreacion = tokenModel.FechaCreacion,
                        TiempoExpira = tokenModel.TiempoExpira
                    });

                    HttpContext.Session.SetString("Token", tokenModel.Token);
                    HttpContext.Session.SetInt32("IdUsuario", usuarioValido.IdUsuario);
                    HttpContext.Session.SetString("Usuario", usuario);
                    HttpContext.Session.SetString("Rol", rol);
                    HttpContext.Session.SetInt32("IdSistema", idSistema);

                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Login Cambio Contraseña",
                        Descripcion = $"Inicio de sesión con cambio de contraseña para el usuario {usuario}.",
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        FK_IdSistema = idSistema
                    });

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
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error LoginCambioContrasenia",
                    Descripcion = $"Error en el proceso de login para usuario {usuario}: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("IndexCambioContrasenia");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SolicitarNuevaContrasenia(string username)
        {
            try
            {
                var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
                if (usuarios == null)
                    return Json(new { success = false, message = "Usuario no encontrado" });

                var usuario = usuarios.FirstOrDefault(u => u.Username == username);
                if (usuario == null)
                    return Json(new { success = false, message = "Usuario no encontrado" });

                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                var correo = empleados.FirstOrDefault(e => e.IdEmpleado == usuario.FK_IdEmpleado);
                if (correo == null)
                    return Json(new { success = false, message = "No se han encontrado correos asociados al usuario" });

                string nuevaContrasenia = daoUsuarioWSAsync.GenerarContraseniaAleatoria();
                string hash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasenia);
                DateTime nuevaExpiracion = DateTime.UtcNow.AddHours(1);

                await _daoUsuario.ActualizarContraseniaExpiracionAsync(usuario.IdUsuario, hash, nuevaExpiracion);

                string urlCambioPassword = "https://localhost:7001/CambioContrasena";

                await _emailService.EnviarCorreoConMailjetAsync(usuario.Username, correo.CorreoInstitucional, nuevaContrasenia, urlCambioPassword);

                return Json(new { success = true, message = "Se ha enviado una nueva contraseña a tu correo." });
            }
            catch (Exception e)
            {
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error SolicitarNuevaContrasenia",
                    Descripcion = $"Error al generar nueva contraseña para {username}: {e.Message}",
                    Estado = false
                });

                return Json(new { success = false, message = "Error al procesar la solicitud." });
            }
        }

        [HttpGet]
        public IActionResult CambioContrasena()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error CambioContrasena GET",
                    Descripcion = $"Error al cargar la página de cambio de contraseña: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al cargar la página. Por favor, inténtelo de nuevo.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambioContrasena(string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            try
            {
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema");

                if (idUsuario == null || string.IsNullOrEmpty(usuario))
                    return RedirectToAction("Index", "Login");

                ViewBag.Usuario = usuario;

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

                if (nuevaContraseña.Length < 8)
                {
                    ViewBag.Mensaje = "La nueva contraseña debe tener al menos 8 caracteres.";
                    return View();
                }

                bool tieneMayuscula = nuevaContraseña.Any(char.IsUpper);
                bool tieneMinuscula = nuevaContraseña.Any(char.IsLower);
                bool tieneNumero = nuevaContraseña.Any(char.IsDigit);

                if (!tieneMayuscula || !tieneMinuscula || !tieneNumero)
                {
                    ViewBag.Mensaje = "La contraseña debe contener al menos una letra mayúscula, una minúscula y un número.";
                    return View();
                }

                var usuarioValido = _daoTokenUsuario.ValidarUsuarioCambioContrasenia(usuario, contraseñaActual);

                if (usuarioValido == null)
                {
                    ViewBag.Mensaje = "La contraseña actual es incorrecta.";
                    return View();
                }

                if (contraseñaActual == nuevaContraseña)
                {
                    ViewBag.Mensaje = "La nueva contraseña debe ser diferente a la actual.";
                    return View();
                }

                string hashNuevaContraseña = BCrypt.Net.BCrypt.HashPassword(nuevaContraseña);

                try
                {
                    _daoTokenUsuario.GuardarContrasenia(usuarioValido.IdUsuario, hashNuevaContraseña);

                    await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                    {
                        Accion = "Cambio de Contraseña",
                        Descripcion = $"El usuario {usuario} ha cambiado su contraseña exitosamente.",
                        FK_IdUsuario = usuarioValido.IdUsuario,
                        FK_IdSistema = idSistema ?? 1
                    });

                    _daoTokenUsuario.RevocarToken(usuarioValido.IdUsuario);
                    HttpContext.Session.Clear();

                    TempData["MensajeExito"] = "Contraseña cambiada exitosamente. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    await _loggingService.RegistrarLogAsync(new LogViewModel
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
                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error CambioContrasena POST",
                    Descripcion = $"Error al procesar el cambio de contraseña: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
