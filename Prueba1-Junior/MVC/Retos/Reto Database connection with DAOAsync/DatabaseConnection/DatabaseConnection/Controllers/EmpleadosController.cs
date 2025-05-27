using DatabaseConnection.Data;
using DatabaseConnection.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseConnection.Controllers
{
	public class EmpleadosController : Controller
	{
		private readonly IDaoEmpleadosAsync _daoEmpleadosAsync;
        public EmpleadosController( IDaoEmpleadosAsync daoEmpleadosAsync)
		{
			_daoEmpleadosAsync = daoEmpleadosAsync;
        }


        [HttpGet]
		public async Task<IActionResult> Index()
		{
			try
			{
				var empleados = await _daoEmpleadosAsync.GetEmpleadosAsync();

				return View(empleados);

            }
            catch (Exception ex)
			{
				// Log the exception (not implemented here)
				return StatusCode(500, "Internal server error: " + ex.Message);
            }
		}


		[HttpGet]
		public  IActionResult InsertEmpleadoAsync()
		{

			return View();
        }


		[HttpPost]
		public async Task<IActionResult> InsertEmpleadoAsync(Empleado empleado)
		{
			if(!ModelState.IsValid) return View(empleado);
			try
			{
				await _daoEmpleadosAsync.InsertEmpleadoAsync(empleado);
				
				return RedirectToAction("Index");
				
            }
            catch (Exception ex)
			{
				return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


    }
}
