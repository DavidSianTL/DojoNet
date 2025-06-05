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

        // Acción para eliminar un empleado - GET (mostrar confirmación)
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
                    TempData["Error"] = "Empleado no encontrado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleado para eliminar: {ex.Message}");
                TempData["Error"] = "Error al cargar la información del empleado.";
                return RedirectToAction("Index");
            }
        }

        // Acción para eliminar un empleado - POST (ejecutar eliminación)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ELIMINAR(int id, IFormCollection collection)
        {
            try
            {
                // Obtener el empleado antes de eliminarlo para mostrar mensaje
                var empleado = await _daoEmpleado.ObtenerEmpleadoPorIdAsync(id);
                
                if (empleado == null)
                {
                    TempData["Error"] = "Empleado no encontrado.";
                    return RedirectToAction("Index");
                }

                // Ejecutar la eliminación (soft delete - cambiar estado a 0)
                await _daoEmpleado.EliminarEmpleadoAsync(id);

                // Mensaje de éxito
                TempData["Success"] = $"El empleado {empleado.NombreEmpleado} {empleado.ApellidoEmpleado} ha sido desactivado exitosamente.";
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                TempData["Error"] = "Error al eliminar el empleado. Por favor, intenta nuevamente.";
                
                // En caso de error, redirigir de vuelta a la vista de confirmación
                return RedirectToAction("ELIMINAR", new { id = id });
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