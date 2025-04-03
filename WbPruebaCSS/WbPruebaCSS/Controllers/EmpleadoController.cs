using Microsoft.AspNetCore.Mvc;
using WbPruebaCSS.Models;
using System.Collections.Generic;
using System.Linq;
namespace WbPruebaCSS.Controllers
{
    public class EmpleadoController : Controller
    {
        // Simularemos una base de datos con una lista de empleados.
        private static List<Empleado_Modeller> empleados = new List<Empleado_Modeller>();

        // Mostrar el formulario para agregar un empleado
        public IActionResult Crear()
        {
            Empleado_Modeller empleado = new Empleado_Modeller();

            return View(empleado);
        }

        // Acción para guardar un empleado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Empleado_Modeller empleado)
        {
            if (ModelState.IsValid)
            {
                // Agregar el empleado a la lista (simulando una base de datos)
                empleado.Id = empleados.Count + 1; // Asignar un ID único
                empleados.Add(empleado);
                return RedirectToAction(nameof(Index)); // Redirige a Index si la operación es exitosa
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Index()
        {
            // Pasar la lista de empleados a la vista, que está almacenada en la lista estática
            return View(empleados); // Usamos la lista de empleados que está en la clase
        }

    }
}
