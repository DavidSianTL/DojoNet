using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using HojadeTrabajoAPI_REST.DAO;
using HojadeTrabajoAPI_REST.Models;
using HojadeTrabajoAPI_REST.Models.Responses;
using HojadeTrabajoAPI_REST.Exceptions;
using Microsoft.AspNetCore.Authorization;
using HojadeTrabajoAPI_REST.Controllers.v1;

namespace HojadeTrabajoAPI_REST.Controllers.v1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly daoPacienteAsync _DaoPacientes;
        private readonly ILogger<PacienteController> _logger;

        public PacienteController(daoPacienteAsync daoPacienteP, ILogger<PacienteController> logger)
        {
            _DaoPacientes = daoPacienteP;
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
        public async Task<ActionResult<List<Paciente>>> Get()
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("Petición GET pacientes hecha por: {Usuario}", usuarioLogueado);

                var pacientes = await _DaoPacientes.ObtenerPacientesAsync();
                return Ok(new ApiResponse<List<Paciente>>(200, $"Pacientes obtenidos correctamente.", pacientes));


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Ocurrió un error al obtener los pacientes");
                return StatusCode(500, new ApiResponse<List<Paciente>>(500, $"Error: {ex.Message}"));

            }
        }

        //EndPoint para POST
        [Authorize]
        [HttpPost("v1")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("Petición POST para insertar paciente hecha por: {Usuario}", usuarioLogueado);

                await _DaoPacientes.InsertarPacienteAsync(paciente);

                return Ok(new ApiResponse<object>(201, "Paciente insertado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Error al insertar paciente");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        //EndPoint para PUT
        [Authorize]
        [HttpPut("v1/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Paciente paciente)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                _logger.LogInformation("Petición PUT pacientes hecha por: {Usuario}", usuarioLogueado);
                paciente.IdPaciente = id;
                await _DaoPacientes.ActualizarPacienteAsync(paciente);
                return Ok(new ApiResponse<object>(200, $"Paciente actualizado correctamente. Petición hecha por: {usuarioLogueado}"));

            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al actualizar un paciente");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch
            {
                _logger.LogError("500 - ERROR: Al actualizar el paciente");
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar el paciente" });
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

                _logger.LogInformation("Petición DELETE paciente hecha por: {Usuario}", usuarioLogueado);
                await _DaoPacientes.EliminarPacienteAsync(id);
                return Ok(new ApiResponse<object>(200, $"Paciente eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al ELIMINAR un paciente, NO SE ENCONTRO");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }

            catch (Exception ex)
            {
                _logger.LogError("500 - ERROR: Al  eliminar un paciente");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }

        }
    }
}
