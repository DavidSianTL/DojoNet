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
    public class MedicosController : ControllerBase
    {
        private readonly daoMedicoAsyncEF _dao;
        private readonly ILogger<MedicosController> _logger;

        public MedicosController(daoMedicoAsyncEF dao, ILogger<MedicosController> logger)
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
                var usuario = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("GET Médicos hecho por: {User}", usuario);

                var medicos = await _dao.ObtenerMedicosAsync();
                return Ok(new ApiResponse<Object>(200, "Médicos obtenidos correctamente", medicos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener médicos");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var medico = await _dao.ObtenerPorIdAsync(id);
                return Ok(new ApiResponse<object>(200, "Médico encontrado", medico));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener médico por id");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Medico medico)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(400, "Datos inválidos"));

            try
            {
                await _dao.CrearMedicoAsync(medico);
                return Ok(new ApiResponse<Medico>(201, "Médico creado correctamente", medico));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear médico");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Medico medico)
        {
            if (medico == null || id != medico.Id)
                return BadRequest(new ApiResponse<object>(400, "Datos inválidos o ID no coincide"));

            try
            {
                await _dao.ActualizarMedicoAsync(medico);
                return Ok(new ApiResponse<object>(200, "Médico actualizado correctamente"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar médico");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _dao.EliminarMedicoAsync(id);
                return Ok(new ApiResponse<object>(200, "Médico eliminado correctamente"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch (InvalidOperationException ioex)
            {
                return BadRequest(new ApiResponse<object>(400, ioex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar médico");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }
    }
}
