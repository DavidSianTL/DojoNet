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
        public class EspecialidadController : ControllerBase
        {
            private readonly daoEspecialidadAsync _DaoEspecialidad;
            private readonly ILogger<EspecialidadController> _logger;

            public EspecialidadController(daoEspecialidadAsync daoEspecialidad, ILogger<EspecialidadController> logger)
            {
                _DaoEspecialidad = daoEspecialidad;
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
            public async Task<ActionResult<List<Especialidad>>> Get()
            {
                try
                {
                    var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                    _logger.LogInformation("Petición GET especialidades hecha por: {Usuario}", usuarioLogueado);

                    var especialidades = await _DaoEspecialidad.ObtenerEspecialidadesAsync();
                    return Ok(new ApiResponse<List<Especialidad>>(200, $"Especialidades obtenidos correctamente.", especialidades));


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "500 - Ocurrió un error al obtener las especialidades");
                    return StatusCode(500, new ApiResponse<List<Especialidad>>(500, $"Error: {ex.Message}"));

                }
            }
        //EndPoint para POST
        [Authorize]
        [HttpPost("v1")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] Especialidad especialidad)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("Petición POST para insertar una especialidad hecha por: {Usuario}", usuarioLogueado);

                await _DaoEspecialidad.InsertarEspecialidadAsync(especialidad);

                return Ok(new ApiResponse<object>(201, "Especialidad insertada correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Error al insertar una especialidad");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        //EndPoint para PUT
        [Authorize]
        [HttpPut("v1/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Especialidad especialidad)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                _logger.LogInformation("Petición PUT especialidad hecha por: {Usuario}", usuarioLogueado);
                especialidad.IdEspecialidad = id;
                await _DaoEspecialidad.ActualizarEspecialidadAsync(especialidad);
                return Ok(new ApiResponse<object>(200, $"Especialidad actualizada correctamente. Petición hecha por: {usuarioLogueado}"));

            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al actualizar una especialidad");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch
            {
                _logger.LogError("500 - ERROR: Al actualizar una especialidad");
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar una especialidad" });
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

                _logger.LogInformation("Petición DELETE especialidad hecha por: {Usuario}", usuarioLogueado);
                await _DaoEspecialidad.EliminarEspecialidadAsync(id);
                return Ok(new ApiResponse<object>(200, $"Especialidad eliminada correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al ELIMINAR una especialidad, NO SE ENCONTRO");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }

            catch (Exception ex)
            {
                _logger.LogError("500 - ERROR: Al  eliminar una especialidad");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }

        }

    }
}
