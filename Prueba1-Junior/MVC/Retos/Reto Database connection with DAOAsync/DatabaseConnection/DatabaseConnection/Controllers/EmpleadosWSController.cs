using DatabaseConnection.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult InsertEmpleadoWSAsync()
        {
            // # falta obtener el modelo para enviar al formulario de la vista

            return View();
        }

    }
}
