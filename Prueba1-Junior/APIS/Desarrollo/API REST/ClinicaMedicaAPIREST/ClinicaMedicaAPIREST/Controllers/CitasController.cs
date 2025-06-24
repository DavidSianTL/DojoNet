using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Data.DTO.CitasDTOs;
using ClinicaMedicaAPIREST.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ILogger<CitasController> _logger;
        private readonly daoCitas _dao;
        public CitasController(daoCitas dao, ILogger<CitasController> logger)
        {
            _logger = logger;
            _dao = dao;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cita>>> ObtenerCitas()
        {
            var citas = new List<Cita>();
            try
            {
                citas = await _dao.GetCitasAsync();
                if (citas.Count == 0) return StatusCode(404, "No se encontraron citas ");

                return Ok(citas);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de citas");
                return StatusCode(500, "Error al obtener la lista de citas");
            }
        }


        [HttpPost]
        public async Task<ActionResult> InsertarCita([FromBody] CitaRequestDTO cita)
        {
            try
            {
                if (!ModelState.IsValid) return StatusCode(400, "El medico ingresado no es valido, verifique sus datos");

                var result = await _dao.AddCitaAsync(cita);
                if (!result) return StatusCode(500, "Hubo un error al intentar agregar la cita");

                return StatusCode(200, "La cita fue agregada con éxito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar cita");
                return StatusCode(500, "Hubo un error al intentar agregar la cita ");
            }
        }


        [HttpPut]
        public async Task<ActionResult<String>> ActualizarCita([FromBody] Cita cita)
        {
            try
            {
                var citas = await _dao.GetCitasByIdAsync(cita.Id);

                var citaValida = citas.FirstOrDefault(P => P.Id == cita.Id);

                if (cita == null) return StatusCode(404, $"No se encontró la cita con el Id: {cita!.Id}");

                var response = await _dao.UpdateCitaAsync(cita!);

                return Ok("Cita editada Correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar cita");
                return StatusCode(500, "Error al editar cita");
            }
        }


        [HttpDelete]
        public async Task<ActionResult<String>> EliminarCita([FromBody] int Id)
        {
            try
            {
                var citas = await _dao.GetCitasAsync();

                var cita = citas.FirstOrDefault(P => P.Id == Id);

                if (cita == null) return StatusCode(404, $"Error la cita no existe o ya fue eliminada. ");

                var response = await _dao.DeleteCitaAsync(cita);
                if (!response) return StatusCode(500, "Error al eliminar la cita");

                return Ok("Cita eliminiada Correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cita");
                return StatusCode(500, "Error al eliminar cita");
            }
        }

    }
}
