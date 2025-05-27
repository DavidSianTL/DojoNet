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



		[HttpGet]
		public async Task<IActionResult> EditarEmpleadoAsync(int Id)
		{
			var empleados = await _daoEmpleadosAsync.GetEmpleadosAsync();

			var validEmpleado = empleados.FirstOrDefault(emp => emp.EmpleadoID == Id);
            return View(validEmpleado);
		}

		[HttpPost]
		public async Task<IActionResult> EditarEmpleadoAsync(Empleado empleado)
		{
			if (!ModelState.IsValid) return View(empleado);
			if(!await _daoEmpleadosAsync.UpdateEmpleadoAsync(empleado)) return View(empleado);

            return RedirectToAction("Index");
        }



		[HttpGet]
		public async Task<IActionResult> EliminarEmpleadoAsync(int Id)
		{
			if (Id <= 0) return View();

			var empleados = await _daoEmpleadosAsync.GetEmpleadosAsync();
			var validEmpleado = empleados.FirstOrDefault(emp => emp.EmpleadoID == Id);

			if (validEmpleado == null) return View();



            return View(validEmpleado);
        }






		[HttpPost]
		public async Task<IActionResult> EliminarEmpleadoAsync(Empleado empleado)
		{
			if(!ModelState.IsValid) return View(empleado);


			if(!await _daoEmpleadosAsync.DeleteEmpleadoAsync(empleado.EmpleadoID)) return View(empleado) ;


			return RedirectToAction("Index");
        }





    }
}
