using Microsoft.AspNetCore.Mvc;
using wAppGestionVacacional.Models;
using wAppGestionVacacional.Services;

namespace wAppGestionVacacional.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoService _empleadoService;

        public EmpleadoController(EmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }
        
        //Vista para crear un nuevo empleado
        public IActionResult Crear()
        {
            return View();
        }
        // Acción para guardar el nuevo empleado
        [HttpPost]
        public IActionResult Crear(Empleado empleado)
        {

                       if (ModelState.IsValid)
            {
                empleado.CalcularDiasVacaciones();
                _empleadoService.GuardarEmpleado(empleado);
                return RedirectToAction("Index", "Home");
            }
            return View(empleado);
        }

        // Acción para mostrar la lista de empleados
        public IActionResult Listar()
        {
            var empleados = _empleadoService.ObtenerEmpleados();
            return View(empleados);
        }

    }
}
