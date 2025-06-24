using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    [Route("api/[Controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly ILogger<PacientesController> _logger;
        private readonly daoPacientes _dao;
        public PacientesController(daoPacientes dao, ILogger<PacientesController> logger)
        {
            _logger = logger;
            _dao = dao;
        }

        public async Task<ActionResult<List<Paciente>>> Index()
        {
            var pacientes = new List<Paciente>();
            try
            {
                pacientes = await _dao.GetPacientesAsync();

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pacientes");
                return StatusCode(400, "Error al obtener la lista de pacientes");
            }
            return Ok(pacientes);
        }
    }
}
