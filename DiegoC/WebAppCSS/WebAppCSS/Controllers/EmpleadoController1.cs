using Microsoft.AspNetCore.Mvc;
using WebAppCSS.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebAppCSS.Controllers
{
    public class EmpleadoController1 : Controller
    {
        //para simular una base de datos
        private static List<Empleado> empleados = new List<Empleado>();

        public IActionResult Crear()
        {
            return View();
        }
        //Guardar empleados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                //agregar el empleado a la lista
                empleados.Add(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
