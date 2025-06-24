using API2.Data.DAOs;
using API2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly PacienteDAO _pacienteDao;

        public PacientesController(PacienteDAO pacienteDao)
        {
            _pacienteDao = pacienteDao;
        }

        // GET: api/Pacientes
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
            var pacientes = await _pacienteDao.ObtenerTodos();
            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Pacientes obtenidos correctamente",
                data = pacientes
            });
        }

        // GET: api/Pacientes/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaciente(int id)
        {
            var paciente = await _pacienteDao.ObtenerPorId(id);
            if (paciente == null)
            {
                return NotFound(new
                {
                    responseCode = 404,
                    responseMessage = "Paciente no encontrado"
                });
            }

            return Ok(new
            {
                responseCode = 200,
                responseMessage = "Paciente obtenido correctamente",
                data = paciente
            });
        }
    }
}
