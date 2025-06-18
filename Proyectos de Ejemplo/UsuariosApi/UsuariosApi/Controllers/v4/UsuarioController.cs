using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UsuariosApi.DAO;
using UsuariosApi.Models;
using Asp.Versioning;
using UsuariosApi.Models.Responses;
using UsuariosApi.Exceptions;
using Microsoft.AspNetCore.Authorization;
//v3 se implementan NotFoundException
//Se implementa ApiResponses con estructura especifica
//Se implementa versionamiento
namespace UsuariosApi.Controllers.v4

{
    [ApiVersion("4.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly daoUsuariosAsync _DaoUsuarios;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(daoUsuariosAsync daoUsuarioP, ILogger<UsuarioController> logger)
        {
            _DaoUsuarios = daoUsuarioP;
            _logger = logger;
        }

        [HttpGet("version4")]
        public IActionResult GetVersion()
        {
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "No version";
            _logger.LogInformation("Petición version: {apiVersion}", apiVersion);
            return Ok(new { version = apiVersion });
        }
        [Authorize]
        [HttpGet("v4")]
        [MapToApiVersion("4.0")]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");
                _logger.LogInformation("Petición GET usuarios hecha por: {Usuario}", usuarioLogueado);
               
                var usuarios = await _DaoUsuarios.ObtenerUsuariosAsync();
                return Ok(new ApiResponse<List<Usuario>>(200, $"Usuarios obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", usuarios));

               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Ocurrió un error al obtener usuarios");
                return StatusCode(500, new ApiResponse<List<Usuario>>(500, $"Error: {ex.Message}"));
                
            }

        }
        [Authorize] 
        [HttpPost("v4")]
        [MapToApiVersion("4.0")]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
                     
            if (!ModelState.IsValid) 
            {
                _logger.LogError("Datos de usuario inválidos al postear un nuevo usuario");
                return BadRequest(new ApiResponse<object>(400, "Datos de usuario inválidos"));
            }

            if (string.IsNullOrWhiteSpace(usuario.UsuarioLg))
            {
                _logger.LogError("El login del usuario es obligatorio al postear un nuevo usuario");
                return BadRequest(new ApiResponse<object>(400, "El login del usuario es obligatorio"));
            }
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición POST usuarios hecha por: {usuarioLogueado}");
                _logger.LogInformation("Petición POST usuarios hecha por: {Usuario}", usuarioLogueado);
                await _DaoUsuarios.InsertarUsuarioAsync(usuario);
                return Ok(new ApiResponse<object>(201, $"Usuario creado correctamente.  Petición hecha por: {usuarioLogueado}"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "500 - Ocurrió un error al postear un nuevo usuario");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }

        }
        [Authorize]
        [HttpPut("v4/{id}")]
        [MapToApiVersion("4.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            
            if (!ModelState.IsValid)
            {
                _logger.LogError("400 - Datos de usuario inválidos al postear un nuevo usuario");
                return BadRequest(new ApiResponse<object>(400, "Datos de usuario inválidos"));
            }
            if (string.IsNullOrWhiteSpace(usuario.UsuarioLg))
            {
                _logger.LogError("400 - El login del usuario es obligatorio al postear un nuevo usuario");
                return BadRequest(new ApiResponse<object>(400, "El nombre de usuario es obligatorio"));
            }
            try
            {
                 var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición PUT usuarios hecha por: {usuarioLogueado}");
                _logger.LogInformation("Petición PUT usuarios hecha por: {Usuario}", usuarioLogueado);
                usuario.IdUsuario = id;
                await _DaoUsuarios.ActualizarUsuarioAsync(usuario);
                return Ok(new ApiResponse<object>(200, $"Usuario actualizado correctamente. Petición hecha por: {usuarioLogueado}"));
               
            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al actualizar un usuario");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch
            {
                _logger.LogError("500 - ERROR: Al actualizar usuario");
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar usuario" });
            }

        }
        [Authorize]
        [HttpDelete("v4/{id}")]
        [MapToApiVersion("4.0")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición DELETE usuarios hecha por: {usuarioLogueado}");
                _logger.LogInformation("Petición DELETE usuarios hecha por: {Usuario}", usuarioLogueado);
                await _DaoUsuarios.EliminarUsuarioAsync(id);
                return Ok(new ApiResponse<object>(200, $"Usuario eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                _logger.LogError(nfex, "404 - Ocurrió un error al ELIMINAR un usuario, NO SE ENCONTRO");
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }

            catch (Exception ex)
            {
                _logger.LogError("500 - ERROR: Al  eliminar usuario");
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }

        }


       
    }
}
