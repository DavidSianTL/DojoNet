using Asp.Versioning;
using HojadeTrabajoAPI_REST.DAO;
using HojadeTrabajoAPI_REST.Models.Responses;
using HojadeTrabajoAPI_REST.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HojadeTrabajoAPI_REST.Exceptions;

namespace HojadeTrabajoAPI_REST.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly daoCitaAsync _DaoCitas;
        private readonly ILogger<CitaController> _logger;

        public CitaController(daoCitaAsync daoCita, ILogger<CitaController> logger)
        {
            _DaoCitas = daoCita;
            _logger = logger;
        }

        [HttpGet("version1")]
        public IActionResult GetVersion()
        {
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "No version";
            _logger.LogInformation("Petición version: {apiVersion}", apiVersion);
            return Ok(new { version = apiVersion });
        }

        //EndPoint para GET
        [Authorize]
        [HttpGet("v1")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<List<Cita>>> Get()
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("Petición GET citas hecha por: {Usuario}", usuarioLogueado);

                var citas = await _DaoCitas.ObtenerCitasAsync();
                return Ok(new ApiResponse<List<Cita>>(200, $"Citas obtenidas correctamente.", citas));


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Ocurrió un error al obtener las citas");
                return StatusCode(500, new ApiResponse<List<Cita>>(500, $"Error: {ex.Message}"));

            }
        }

        //EndPoint para POST
        [Authorize]
        [HttpPost("v1")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("Petición POST para insertar cita hecha por: {Usuario}", usuarioLogueado);

                await _DaoCitas.InsertarCitaAsync(cita);

                return Ok(new ApiResponse<object>(201, "Cita insertada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Error al insertar cita");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        //EndPoint para PUT
        [Authorize]
        [HttpPut("v1/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Cita cita)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                _logger.LogInformation("Petición PUT citas hecha por: {Usuario}", usuarioLogueado);
                cita.IdCita = id;
                await _DaoCitas.ActualizarCitaAsync(cita);
                return Ok(new ApiResponse<object>(200, $"Cita actualizada correctamente. Petición hecha por: {usuarioLogueado}"));

            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al actualizar una cita");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch
            {
                _logger.LogError("500 - ERROR: Al actualizar una cita");
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar una cita" });
            }
        }

        //EndPoint para DELETE
        [Authorize]
        [HttpDelete("v1/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                _logger.LogInformation("Petición DELETE cita hecha por: {Usuario}", usuarioLogueado);
                await _DaoCitas.EliminarCitaAsync(id);
                return Ok(new ApiResponse<object>(200, $"Cita eliminada correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al ELIMINAR una cita, NO SE ENCONTRO");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }

            catch (Exception ex)
            {
                _logger.LogError("500 - ERROR: Al  eliminar una cita");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }
    }
}
