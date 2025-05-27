using DatabaseConnection.Data;
using DatabaseConnection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DatabaseConnection.Controllers
{
    public class EmpleadosWSController : Controller
    {

        private readonly IDaoEmpleadosAsyncWS _daoEmpleadosAsyncWS;
        public EmpleadosWSController( IDaoEmpleadosAsyncWS daoEmpleadosAsyncWS) 
        {
            _daoEmpleadosAsyncWS = daoEmpleadosAsyncWS;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var empleados = await _daoEmpleadosAsyncWS.GetEmpleadosListAsync();

            return View(empleados);
        }



        [HttpGet]
        public async Task<IActionResult> EliminarEmpleadoWSAsync(int Id)
        {
            var empleados = await _daoEmpleadosAsyncWS.GetEmpleadosListAsync();
            var validEmpleado = empleados.FirstOrDefault(emp => emp.EmpleadoID == Id);

            return View(validEmpleado);
        }

            [HttpPost]
        public async Task<IActionResult> EliminarEmpleadoWSAsync(Empleado empleado)
        {
            // # falta obtener el modelo para enviar al formulario de la vista

            try
            {

                if(!await _daoEmpleadosAsyncWS.DeleteEmpleadoAsync(empleado.EmpleadoID)) return View(empleado.EmpleadoID); // Si no se pudo eliminar, retornamos la vista con el Id del empleado

            }
            catch (Exception ex)
            {
                Console.WriteLine("El empleado no se pudo eliminar error: " + ex);
            }

            return RedirectToAction("Index");
        }





    }
}
