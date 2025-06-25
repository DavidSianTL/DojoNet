using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Data.DTO.MedicosDTOs;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly ILogger<MedicosController> _logger;
        private readonly daoMedicos _dao;
        public MedicosController(daoMedicos dao, ILogger<MedicosController> logger)
        {
            _logger = logger;
            _dao = dao;
        }

        [HttpGet]
        [Authorize(Roles = "sysAdmin")]
        public async Task<ActionResult<List<Medico>>> ObtenerMedicos()
        {
            var medicos = new List<Medico>();
            try
            {
                medicos = await _dao.GetMedicosAsync();
                if (medicos.Count == 0) return StatusCode(404, "No se encontraron medicos ");
                return Ok(medicos);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de medicos");
                return StatusCode(500, "Error al obtener la lista de medicos");
            }
        }


        [HttpPost]
        [Authorize(Roles = "sysAdmin")]
        public async Task<ActionResult> InsertarMedico([FromBody] MedicoRequestDTO medico)
        {
            try
            {
                if (!ModelState.IsValid) return StatusCode(400, "El medico ingresado no es valido, verifique sus datos");

                var result = await _dao.AddMedicoAsync(medico);
                if (!result) return StatusCode(500, "Hubo un error al intentar agregar al medico");

                return StatusCode(200, "El medico fue agregado con éxito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar medico");
                return StatusCode(500, "Hubo un error al intentar agregar al medico ");
            }
        }


        [HttpPut]
        [Authorize(Roles = "sysAdmin")]
        public async Task<ActionResult<String>> ActualizarMedico([FromBody] Medico medico)
        {
            try
            {
                var medicos = await _dao.GetMedicosByIdAsync(medico.Id);

                var medicoValido = medicos.FirstOrDefault(P => P.Id == medico.Id);

                if (medico == null) return StatusCode(404, $"No se encontró el medico con el Id: {medico!.Id}");

                var response = await _dao.UpdateMedicoAsync(medico!);

                return Ok("Medico Editado Correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar medico");
                return StatusCode(500, "Error al editar medico");
            }
        }


        [HttpDelete]
        [Authorize(Roles = "sysAdmin")]
        public async Task<ActionResult<String>> EliminarMedico([FromBody] int Id)
        {
            try
            {
                var medicos = await _dao.GetMedicosAsync();

                var medico = medicos.FirstOrDefault(P => P.Id == Id);

                if (medico == null) return StatusCode(404, $"Error el medico no existe o ya fue eliminado. ");

                var response = await _dao.DeleteMedicoAsync(medico);

                return Ok("Medico eliminiado Correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar medico");
                return StatusCode(500, "Error al eliminar medico");
            }
        }

    }
}
