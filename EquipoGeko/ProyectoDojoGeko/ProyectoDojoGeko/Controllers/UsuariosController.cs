using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Services.Interfaces;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class UsuariosController : Controller
    {
        private readonly daoUsuarioWSAsync _daoUsuarioWS;
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly ILoggingService _loggingService;
        private readonly EmailService _emailService;

        public UsuariosController(
            daoUsuarioWSAsync daoUsuarioWS,
            daoEmpleadoWSAsync daoEmpleado,
            daoBitacoraWSAsync daoBitacora,
            ILoggingService loggingService,
            EmailService emailService)
        {
            _daoUsuarioWS = daoUsuarioWS;
            _daoEmpleado = daoEmpleado;
            _daoBitacora = daoBitacora;
            _loggingService = loggingService;
            _emailService = emailService;
        }
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _daoUsuarioWS.ObtenerUsuariosAsync();
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                var model = usuarios?.Select(u => new UsuarioFormViewModel
                {
                    Usuario = u,
                    Empleados = empleados?.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                    }).ToList() ?? new List<SelectListItem>()
                }).ToList() ?? new List<UsuarioFormViewModel>();

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario");
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Vista Usuarios",
                    Descripcion = $"Ingreso a la vista de usuarios por {usuario}",
                    FK_IdUsuario = idUsuario,
                    FK_IdSistema = idSistema
                });

                return View(model);
            }
            catch (Exception e)
            {
                var usuario = HttpContext.Session.GetString("Usuario");

                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Vista Usuarios",
                    Descripcion = $"Error en el ingreso a la vista de usuarios del {usuario}: {e.Message}.",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View(new List<UsuarioFormViewModel>());
            }
        }
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            try
            {
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                var model = new UsuarioFormViewModel
                {
                    Empleados = empleados.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                    }).ToList()
                };

                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

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
                var usuario = HttpContext.Session.GetString("Usuario");

                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Crear Usuario",
                    Descripcion = $"Error al ingresar a la vista de crear usuario del {usuario}: {e.Message}.",
                    Estado = false
                });

                ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View();
            }
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                    model.Empleados = empleados.Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = $"{e.NombreEmpleado} {e.ApellidoEmpleado}"
                    }).ToList();

                    ViewBag.Error = "Error en los datos para crear el usuario";
                    return View(model);
                }

                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var usuarioActual = HttpContext.Session.GetString("Usuario") ?? "Sistema";
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                var (idUsuarioCreado, contraseniaGenerada) = await _daoUsuarioWS.InsertarUsuarioAsync(model.Usuario);

                var emailDestino = "jperalta@digitalgeko.com";

                var urlCambioPassword = Url.Action(
                    "IndexCambioContrasenia",
                    "Login",
                    new { id = model.Usuario.IdUsuario },
                    protocol: Request.Scheme
                );

                await _emailService.EnviarCorreoConMailjetAsync(model.Usuario.Username, emailDestino, contraseniaGenerada, urlCambioPassword);

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Crear Usuario",
                    Descripcion = $"Nuevo usuario '{model.Usuario.Username}' creado por {usuarioActual}.",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = idSistema
                });

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var usuarioSesion = HttpContext.Session.GetString("Usuario");

                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Crear Usuario",
                    Descripcion = $"Error al intentar crear un nuevo usuario '{model.Usuario?.Username}' por {usuarioSesion}: {e.Message}.",
                    Estado = false
                });

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
                var usuario = await _daoUsuarioWS.ObtenerUsuarioPorIdAsync(id);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

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
            catch (Exception e)
            {
                var usuarioSession = HttpContext.Session.GetString("Usuario");

                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Cargar Editar Usuario",
                    Descripcion = $"Error al cargar usuario ID {id} para edición por {usuarioSession}: {e.Message}",
                    Estado = false
                });

                TempData["Error"] = "Error al cargar el usuario para edición";
                return RedirectToAction("Index");
            }
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioViewModel usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
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

                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Editar Usuario",
                    Descripcion = $"Usuario {usuario.Username} editado",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = idSistema
                });

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var usuarioSession = HttpContext.Session.GetString("Usuario");

                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Editar Usuario",
                    Descripcion = $"Error al editar usuario ID {usuario.IdUsuario} por {usuarioSession}: {e.Message}",
                    Estado = false
                });

                TempData["Error"] = "Error al editar el usuario. Por favor, inténtelo de nuevo.";
                return RedirectToAction("Index");
            }
        }

        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    ViewBag.Mensaje = "ID de usuario inválido.";
                    return View("Index", "Usuarios");
                }

                var usuario = await _daoUsuarioWS.ObtenerUsuarioPorIdAsync(Id);

                if (usuario == null)
                {
                    ViewBag.Mensaje = "Usuario no encontrado.";
                    return View("Index", "Usuarios");
                }

                await _daoUsuarioWS.EliminarUsuarioAsync(usuario.IdUsuario);

                var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
                {
                    Accion = "Eliminar Usuario",
                    Descripcion = $"El usuario con ID {usuario.IdUsuario} ha sido eliminado.",
                    FK_IdUsuario = idUsuarioSesion,
                    FK_IdSistema = idSistema
                });

                ViewBag.Mensaje = "Usuario eliminado correctamente.";
                return View("Index", "Usuarios");
            }
            catch (Exception e)
            {
                var usuarioSesion = HttpContext.Session.GetString("Usuario");

                await _loggingService.RegistrarLogAsync(new LogViewModel
                {
                    Accion = "Error Eliminar Usuario",
                    Descripcion = $"Error al eliminar el usuario con ID {Id} por {usuarioSesion}: {e.Message}",
                    Estado = false
                });

                ViewBag.Mensaje = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Index", "Usuarios");
            }
        }
    }
}


