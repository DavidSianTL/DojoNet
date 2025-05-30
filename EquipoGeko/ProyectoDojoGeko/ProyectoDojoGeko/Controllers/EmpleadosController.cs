using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; //  Cambio: usar Microsoft.Data.SqlClient en lugar de System.Data.SqlClient
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly string connectionString;
        private readonly daoEmpleadoWSAsync _daoEmpleado;

        public EmpleadosController()
        {
            // Cadena de conexión a la base de datos - ACTUALIZADA
            connectionString = "Server=DARLA\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos el DAO con la cadena de conexión
            _daoEmpleado = new daoEmpleadoWSAsync(connectionString);
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
    }
}
