using Azure.Core;
using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Data.DTO.PacientesDTOs;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly ILogger<PacientesController> _logger;
        private readonly daoPacientes _dao;
        public PacientesController(daoPacientes dao, ILogger<PacientesController> logger)
        {
            _logger = logger;
            _dao = dao;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paciente>>> ObtenerPacientes()
        {
            var pacientes = new List<Paciente>();
            try
            {
                pacientes = await _dao.GetPacientesAsync();
                if (pacientes.Count == 0) return StatusCode(404,"No se encontraron pacientes ");
                return Ok(pacientes);

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pacientes");
                return StatusCode(500, "Error al obtener la lista de pacientes");
            }
        }


        [HttpPost]
        public async Task<ActionResult> InsertarPaciente([FromBody] PacienteRequestDTO paciente)
        {
            try
            {
                if (!ModelState.IsValid) return StatusCode(400, "El paciente ingresado no es valido, verifique sus datos");

                var result = await _dao.AddPacienteAsync(paciente);
                if (!result) return StatusCode(500, "Hubo un error al intentar agregar al paciente");

                return StatusCode(200, "El usuario fue agregado con éxito");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al agregar usuario");
                return StatusCode(500, "Hubo un error al intentar agregar al paciente ");
            }
        }


        [HttpPut]
        public async Task<ActionResult<String>> ActualizarPaciente([FromBody] Paciente paciente)
        {
            try
            {
                var pacientes = await _dao.GetPacientesByIdAsync(paciente.Id);

                var pacienteValido = pacientes.FirstOrDefault(P => P.Id == paciente.Id);

                if (paciente == null) return StatusCode(404, $"No se encontró el paciente con el Id: {paciente!.Id}");

                var response = await _dao.UpdatePacienteAsync(paciente!);

                return Ok("Paciente Editado Correctamente");
            }catch(Exception ex){
                _logger.LogError(ex, "Error al editar paciente");
                return StatusCode(500, "Error al editar paciente");
            }
        }


        [HttpDelete]
        public async Task<ActionResult<String>> EliminarPaciente([FromBody] int Id)
        {
            try
            {
                var pacientes = await _dao.GetPacientesAsync();

                var paciente = pacientes.FirstOrDefault(P => P.Id == Id);

                if (paciente == null) return StatusCode(404, $"Error el paciente no existe o ya fue eliminado. ");

                var response = await _dao.DeletePacienteAsync(paciente);

                return Ok("Paciente eliminiado Correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar paciente");
                return StatusCode(500, "Error al eliminar paciente");
            }
        }
    }
}
