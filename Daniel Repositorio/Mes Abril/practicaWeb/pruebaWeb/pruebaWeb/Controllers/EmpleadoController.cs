using Microsoft.AspNetCore.Mvc;
using pruebaWeb.Models; 
using System.Collections.Generic;
using System.Linq;

namespace pruebaWeb.Controllers
{
    public class EmpleadoController : Controller
    {
        //simularemos una base de datos con listas de empleados
        private static List<EmpleadoModelo> empleados = new List<EmpleadoModelo>();
        public IActionResult Crear()
        {
            return View(empleados);
        }
        //Guardar empleados
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Crear(EmpleadoModelo empleado)
        {
            if (ModelState.IsValid)
            {
                empleados.Add(new Empleado
                {
                    //Agregar empleados a la lista
                    empleado.Id, = empleados.Count + 1,
                    empleados.Add(empleado);
                    return RedirectToAction(nameof(Index);
            });
                return RedirectToAction("Index");
            }
            return View(empleados);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
