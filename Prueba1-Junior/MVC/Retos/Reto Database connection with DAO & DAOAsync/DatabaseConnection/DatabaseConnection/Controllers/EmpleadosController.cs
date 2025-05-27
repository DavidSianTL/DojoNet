using DatabaseConnection.Data;
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
		public async Task<IActionResult> InsertEmpleadoAsync()
		{



			return View();
        }








	}
}
