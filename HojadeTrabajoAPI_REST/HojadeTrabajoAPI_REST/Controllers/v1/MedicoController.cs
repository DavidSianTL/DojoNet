using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
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
        public class MedicoController : ControllerBase
        {
            private readonly daoMedicoAsync _DaoMedicos;
            private readonly ILogger<MedicoController> _logger;

            public MedicoController(daoMedicoAsync daoMedico, ILogger<MedicoController> logger)
            {
                _DaoMedicos = daoMedico;
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
            public async Task<ActionResult<List<Medico>>> Get()
            {
                try
                {
                    var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                    _logger.LogInformation("Petición GET medicos hecha por: {Usuario}", usuarioLogueado);

                    var medicos = await _DaoMedicos.ObtenerMedicosAsync();
                    return Ok(new ApiResponse<List<Medico>>(200, $"Medicos obtenidos correctamente.", medicos));


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "500 - Ocurrió un error al obtener los medicos");
                    return StatusCode(500, new ApiResponse<List<Medico>>(500, $"Error: {ex.Message}"));

                }
            }

        //EndPoint para POST
        [Authorize]
        [HttpPost("v1")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Post([FromBody] Medico medico)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                _logger.LogInformation("Petición POST para insertar un medico hecha por: {Usuario}", usuarioLogueado);

                await _DaoMedicos.InsertarMedicoAsync(medico);

                return Ok(new ApiResponse<object>(201, "Medico insertado correctamente."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Error al insertar un medico");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }
        }

        //EndPoint para PUT
        [Authorize]
        [HttpPut("v1/{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Medico medico)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                _logger.LogInformation("Petición PUT medicos hecha por: {Usuario}", usuarioLogueado);
                medico.IdMedico = id;
                await _DaoMedicos.ActualizarMedicoAsync(medico);
                return Ok(new ApiResponse<object>(200, $"Medico actualizado correctamente. Petición hecha por: {usuarioLogueado}"));

            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al actualizar un medico");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch
            {
                _logger.LogError("500 - ERROR: Al actualizar el medico");
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar el medico" });
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

                _logger.LogInformation("Petición DELETE medico hecha por: {Usuario}", usuarioLogueado);
                await _DaoMedicos.EliminarMedicoAsync(id);
                return Ok(new ApiResponse<object>(200, $"Medico eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al ELIMINAR un medico, NO SE ENCONTRO");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }

            catch (Exception ex)
            {
                _logger.LogError("500 - ERROR: Al  eliminar un medico");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }

        }
    }
}
