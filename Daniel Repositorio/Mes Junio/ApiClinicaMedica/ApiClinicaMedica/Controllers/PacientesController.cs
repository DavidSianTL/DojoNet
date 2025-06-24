using ApiClinicaMedica.Dao;
using ApiClinicaMedica.Models;
using ApiClinicaMedica.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiClinicaMedica.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v4/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly PacienteDAO _dao;

        public PacientesController(PacienteDAO dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pacientes = await _dao.ObtenerTodosAsync();
            return Ok(new ApiResponse<List<Paciente>>(200, "Listado de pacientes", pacientes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var paciente = await _dao.ObtenerPorIdAsync(id);
            if (paciente == null)
                return NotFound(new ApiResponse<string>(404, "Paciente no encontrado"));
            return Ok(new ApiResponse<Paciente>(200, "Paciente encontrado", paciente));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Paciente p)
        {
            await _dao.CrearAsync(p);
            return Ok(new ApiResponse<Paciente>(200, "Paciente creado correctamente", p));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Paciente p)
        {
            var actualizado = await _dao.ActualizarAsync(id, p);
            if (!actualizado)
                return NotFound(new ApiResponse<string>(404, "Paciente no encontrado"));
            return Ok(new ApiResponse<string>(200, "Paciente actualizado correctamente"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _dao.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new ApiResponse<string>(404, "Paciente no encontrado"));
            return Ok(new ApiResponse<string>(200, "Paciente eliminado correctamente"));
        }
    }
}
