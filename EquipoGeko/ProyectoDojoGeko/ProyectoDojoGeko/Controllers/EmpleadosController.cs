using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly string connectionString;
        private readonly daoEmpleadoWSAsync _daoEmpleado;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoRolesWSAsync _daoRoles;
        private readonly daoSistemaWSAsync _daoSistema;

        public EmpleadosController()
        {
            // Cadena de conexión a la base de datos - ACTUALIZADA
            string connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            // Inicializamos los DAOs con la cadena de conexión
            _daoEmpleado = new daoEmpleadoWSAsync(connectionString);
            _daoUsuario = new daoUsuarioWSAsync(connectionString);
            _daoRoles = new daoRolesWSAsync(connectionString);
            _daoSistema = new daoSistemaWSAsync(connectionString);
        }

        // Vista principal de empleados
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Intentar obtener empleados usando el DAO
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                if (empleados != null && empleados.Any())
                {
                    return View(empleados);
                }
                else
                {
                    // Si no hay empleados, crear una lista vacía para mostrar la vista
                    return View(new List<EmpleadoViewModel>());
                }
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error al obtener empleados: {ex.Message}");

                // En caso de error, mostrar datos de prueba para la presentación
                var empleadosPrueba = ObtenerEmpleadosPrueba();
                ViewBag.Error = "Usando datos de prueba - Error de conexión a la base de datos";
                return View(empleadosPrueba);
            }
        }

        // Método para obtener datos de prueba
        private List<EmpleadoViewModel> ObtenerEmpleadosPrueba()
        {
            return new List<EmpleadoViewModel>
            {
                new EmpleadoViewModel
                {
                    IdEmpleado = 1,
                    NombreEmpleado = "Juan Carlos",
                    ApellidoEmpleado = "García López",
                    DPI = "2547896321478",
                    CorreoInstitucional = "juan.garcia@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-6),
                    Genero = "Masculino",
                    Estado = true
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 2,
                    NombreEmpleado = "María Elena",
                    ApellidoEmpleado = "Rodríguez Morales",
                    DPI = "1987654321098",
                    CorreoInstitucional = "maria.rodriguez@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-12),
                    Genero = "Femenino",
                    Estado = true
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 3,
                    NombreEmpleado = "Carlos Alberto",
                    ApellidoEmpleado = "Mendoza Castillo",
                    DPI = "3698521470258",
                    CorreoInstitucional = "carlos.mendoza@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-3),
                    Genero = "Masculino",
                    Estado = false
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 4,
                    NombreEmpleado = "Ana Sofía",
                    ApellidoEmpleado = "Hernández Vega",
                    DPI = "1472583690147",
                    CorreoInstitucional = "ana.hernandez@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-8),
                    Genero = "Femenino",
                    Estado = true
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 5,
                    NombreEmpleado = "Roberto",
                    ApellidoEmpleado = "Jiménez Flores",
                    DPI = "2583691470258",
                    CorreoInstitucional = "roberto.jimenez@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-15),
                    Genero = "Masculino",
                    Estado = true
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 6,
                    NombreEmpleado = "Lucía",
                    ApellidoEmpleado = "Vargas Sánchez",
                    DPI = "9876543210987",
                    CorreoInstitucional = "lucia.vargas@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-4),
                    Genero = "Femenino",
                    Estado = true
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 7,
                    NombreEmpleado = "Diego",
                    ApellidoEmpleado = "Ramírez Torres",
                    DPI = "1357924680135",
                    CorreoInstitucional = "diego.ramirez@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-2),
                    Genero = "Masculino",
                    Estado = false
                },
                new EmpleadoViewModel
                {
                    IdEmpleado = 8,
                    NombreEmpleado = "Gabriela",
                    ApellidoEmpleado = "Cruz Moreno",
                    DPI = "2468135792468",
                    CorreoInstitucional = "gabriela.cruz@geko.com",
                    FechaIngreso = DateTime.Now.AddMonths(-10),
                    Genero = "Femenino",
                    Estado = true
                }
            };
        }

        // Acción para ver detalles de un empleado
        [HttpGet]
        public async Task<IActionResult> LISTAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);

                if (empleado != null)
                {
                    return View(empleado);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleado: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Acción para mostrar detalles completos de un empleado
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);

                if (empleado != null)
                {
                    return View(empleado);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener detalle del empleado: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Acción para asociar usuario a un empleado - GET
        /*[HttpGet]
        public async Task<IActionResult> AsociarUsuario(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);

                if (empleado == null)
                {
                    return NotFound();
                }

                // Verificar que el empleado no tenga usuario asociado
                var usuarioExistente = await _daoUsuario.ObtenerUsuarioPorEmpleadoAsync(id);
                if (usuarioExistente != null)
                {
                    TempData["ErrorMessage"] = "Este empleado ya tiene un usuario asociado";
                    return RedirectToAction("Detalle", new { id = id });
                }

                var model = new AsociarUsuarioViewModel
                {
                    Empleado = empleado,
                    Usuario = new UsuarioViewModel
                    {
                        FK_IdEmpleado = empleado.IdEmpleado,
                        Estado = true,
                        FechaCreacion = DateTime.Now
                    }
                };

                // Cargar listas para los dropdowns
                try
                {
                    var roles = await _daoRoles.ObtenerRolesAsync();
                    var sistemas = await _daoSistema.ObtenerSistemasAsync();

                    model.RolesDisponibles = roles?.ToList() ?? new List<RolViewModel>();
                    model.SistemasDisponibles = sistemas?.ToList() ?? new List<SistemaViewModel>();
                }
                catch (Exception)
                {
                    // En caso de error, usar datos por defecto
                    model.RolesDisponibles = new List<RolViewModel>
                    {
                        new RolViewModel { IdRol = 1, NombreRol = "Administrador" },
                        new RolViewModel { IdRol = 2, NombreRol = "Supervisor" },
                        new RolViewModel { IdRol = 3, NombreRol = "Operador" },
                        new RolViewModel { IdRol = 4, NombreRol = "Consultor" }
                    };

                    model.SistemasDisponibles = new List<SistemaViewModel>
                    {
                        new SistemaViewModel { IdSistema = 1, Nombre = "GEKO - Sistema Principal" },
                        new SistemaViewModel { IdSistema = 2, Nombre = "GEKO - Gestión de Inventarios" },
                        new SistemaViewModel { IdSistema = 3, Nombre = "GEKO - Recursos Humanos" },
                        new SistemaViewModel { IdSistema = 4, Nombre = "GEKO - Contabilidad" }
                    };
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar vista de asociar usuario: {ex.Message}");
                return RedirectToAction("Detalle", new { id = id });
            }
        }

        // Acción para asociar usuario a un empleado - POST
        [HttpPost]
        public async Task<IActionResult> AsociarUsuario(AsociarUsuarioViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Recargar listas en caso de error
                    await CargarListasAsociacion(model);
                    return View(model);
                }

                // Verificar que el empleado existe y no tenga usuario
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(model.Empleado.IdEmpleado);
                if (empleado == null)
                {
                    ModelState.AddModelError("", "Empleado no encontrado");
                    return View(model);
                }

                var usuarioExistente = await _daoUsuario.ObtenerUsuarioPorEmpleadoAsync(model.Empleado.IdEmpleado);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("", "Este empleado ya tiene un usuario asociado");
                    await CargarListasAsociacion(model);
                    return View(model);
                }

                // Verificar que el username no existe
                var usernameExistente = await _daoUsuario.VerificarUsernameExisteAsync(model.Usuario.Username);
                if (usernameExistente)
                {
                    ModelState.AddModelError("Usuario.Username", "Este nombre de usuario ya existe");
                    await CargarListasAsociacion(model);
                    return View(model);
                }

                // Encriptar contraseña
                model.Usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(model.ConfirmarContrasenia);

                // Crear el usuario
                var idUsuarioCreado = await _daoUsuario.InsertarUsuarioAsync(model.Usuario);

                if (idUsuarioCreado > 0)
                {
                    // Simular envío de correo si está habilitado
                    if (model.EnviarCredencialesPorCorreo)
                    {
                        // Aquí iría la lógica para enviar correo
                        // await EnviarCredencialesPorCorreo(empleado, model.Usuario.Username, model.ConfirmarContrasenia);
                    }

                    TempData["SuccessMessage"] = $"Usuario '{model.Usuario.Username}' creado exitosamente para {empleado.NombreEmpleado} {empleado.ApellidoEmpleado}";
                    return RedirectToAction("Detalle", new { id = model.Empleado.IdEmpleado });
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el usuario");
                    await CargarListasAsociacion(model);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al asociar usuario: {ex.Message}");
                ModelState.AddModelError("", "Error interno del servidor");
                await CargarListasAsociacion(model);
                return View(model);
            }
        }

        // Método auxiliar para cargar listas
        private async Task CargarListasAsociacion(AsociarUsuarioViewModel model)
        {
            try
            {
                var roles = await _daoRoles.ObtenerRolesAsync();
                var sistemas = await _daoSistema.ObtenerSistemasAsync();

                model.RolesDisponibles = roles?.ToList() ?? new List<RolViewModel>();
                model.SistemasDisponibles = sistemas?.ToList() ?? new List<SistemaViewModel>();
            }
            catch (Exception)
            {
                // Datos por defecto en caso de error
                model.RolesDisponibles = new List<RolViewModel>
                {
                    new RolViewModel { IdRol = 1, NombreRol = "Administrador" },
                    new RolViewModel { IdRol = 2, NombreRol = "Supervisor" },
                    new RolViewModel { IdRol = 3, NombreRol = "Operador" },
                    new RolViewModel { IdRol = 4, NombreRol = "Consultor" }
                };

                model.SistemasDisponibles = new List<SistemaViewModel>
                {
                    new SistemaViewModel { IdSistema = 1, Nombre = "GEKO - Sistema Principal" },
                    new SistemaViewModel { IdSistema = 2, Nombre = "GEKO - Gestión de Inventarios" },
                    new SistemaViewModel { IdSistema = 3, Nombre = "GEKO - Recursos Humanos" },
                    new SistemaViewModel { IdSistema = 4, Nombre = "GEKO - Contabilidad" }
                };
            }
        }*/

        // Acción para crear un nuevo empleado
        [HttpGet]
        public IActionResult CREAR()
        {
            return View(new EmpleadoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CREAR(EmpleadoViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoEmpleado.InsertarEmpleadoAsync(empleado);
                    return RedirectToAction("Index");
                }

                return View(empleado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear empleado: {ex.Message}");
                ViewBag.Error = "Error al crear el empleado";
                return View(empleado);
            }
        }

        // Acción para editar un empleado
        [HttpGet]
        public async Task<IActionResult> EDITAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);

                if (empleado != null)
                {
                    return View(empleado);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleado para editar: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EDITAR(EmpleadoViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _daoEmpleado.ActualizarEmpleadoAsync(empleado);
                    return RedirectToAction("Index");
                }

                return View(empleado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar empleado: {ex.Message}");
                ViewBag.Error = "Error al editar el empleado";
                return View(empleado);
            }
        }

        // Acción para eliminar un empleado
        [HttpGet]
        public async Task<IActionResult> ELIMINAR(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);

                if (empleado != null)
                {
                    return View(empleado);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleado para eliminar: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ELIMINAR(EmpleadoViewModel empleado)
        {
            try
            {
                await _daoEmpleado.EliminarEmpleadoAsync(empleado.IdEmpleado);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                ViewBag.Error = "Error al eliminar el empleado";
                return View(empleado);
            }
        }

        // API endpoints para datos JSON (opcional)
        [HttpGet]
        public async Task<IActionResult> GetEmpleados()
        {
            try
            {
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                return Json(empleados);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpleado(int id)
        {
            try
            {
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                return Json(empleado);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Añadir la acción GET para CreateEdit que maneja tanto creación como edición
        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            try
            {
                // Si hay un ID, es una edición
                if (id.HasValue && id.Value > 0)
                {
                    var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id.Value);

                    if (empleado != null)
                    {
                        return View(empleado);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    // Si no hay ID, es una creación
                    return View(new EmpleadoViewModel());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar empleado para formulario: {ex.Message}");
                ViewBag.Error = "Error al cargar los datos del empleado";
                return View(new EmpleadoViewModel());
            }
        }

        // Añadir la acción POST para CreateEdit que maneja tanto creación como edición
        [HttpPost]
        public async Task<IActionResult> CreateEdit(EmpleadoViewModel empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (empleado.IdEmpleado > 0)
                    {
                        // Es una actualización
                        await _daoEmpleado.ActualizarEmpleadoAsync(empleado);
                        TempData["SuccessMessage"] = "Empleado actualizado correctamente";
                    }
                    else
                    {
                        // Es una creación
                        await _daoEmpleado.InsertarEmpleadoAsync(empleado);
                        TempData["SuccessMessage"] = "Empleado creado correctamente";
                    }

                    return RedirectToAction("Index");
                }

                return View(empleado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar empleado: {ex.Message}");
                ViewBag.Error = empleado.IdEmpleado > 0
                    ? "Error al actualizar el empleado"
                    : "Error al crear el empleado";
                return View(empleado);
            }
        }
    }
}
