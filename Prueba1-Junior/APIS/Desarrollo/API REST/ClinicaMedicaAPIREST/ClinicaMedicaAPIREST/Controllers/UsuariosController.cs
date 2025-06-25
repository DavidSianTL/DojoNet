using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		private readonly daoUsuarios _daoUsr;
        public UsuariosController(daoUsuarios daoUsr)
        {
			_daoUsr = daoUsr;

        }


        [HttpGet]
		public async Task<ActionResult<List<Usuario>>> ObtenerUsuarios()
		{
            var usuarios = await _daoUsr.GetUsuariosAsync();
			if (usuarios.Count == 0) return StatusCode(404, "No se encontraron usuarios");

			return Ok(usuarios);
        }

		
	}
}
