using Microsoft.AspNetCore.Mvc;
using ClinicaApi.DAO;
using ClinicaApi.Models;
using ClinicaApi.Models.Responses;
using ClinicaApi.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ClinicaApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly daoPacienteAsyncEF _dao;
        private readonly ILogger<PacientesController> _logger;

        public PacientesController(daoPacienteAsyncEF dao, ILogger<PacientesController> logger)
        {
            _dao = dao;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var pacienteLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("GET Pacientes hecho por: {User}", pacienteLogueado);

                var pacientes = await _dao.ObtenerPacientesAsync();
                return Ok(new ApiResponse<object>(200, "Pacientes obtenidos correctamente", pacientes));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pacientes");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<object> ObtenerPorIdAsync(int id)
        {
            try
            {
                var paciente = await _dao.ObtenerPorIdAsync(id);
                return Ok(new ApiResponse<object>(200, "Paciente encontrado", paciente));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener paciente por id");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(400, "Datos inválidos"));

            try
            {
                await _dao.CrearPacienteAsync(paciente);
                return Ok(new ApiResponse<Paciente>(201, "Paciente creado correctamente", paciente));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear paciente");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Paciente paciente)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(400, "Datos inválidos"));

            paciente.Id = id;

            try
            {
                await _dao.ActualizarPacienteAsync(paciente);
                return Ok(new ApiResponse<object>(200, "Paciente actualizado correctamente"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar paciente");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _dao.EliminarPacienteAsync(id);
                return Ok(new ApiResponse<object>(200, "Paciente eliminado correctamente"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch (InvalidOperationException ioex) // ← Atrapamos el error de integridad
            {
                return BadRequest(new ApiResponse<object>(400, ioex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar paciente");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }
    }
}
