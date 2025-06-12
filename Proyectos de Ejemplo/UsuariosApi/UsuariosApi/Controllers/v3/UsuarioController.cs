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
//v3 se implementan NotFoundException
//Se implementa ApiResponses con estructura especifica
//Se implementa versionamiento
namespace UsuariosApi.Controllers.v3

{
    [ApiVersion("3.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly daoUsuariosAsync _DaoUsuarios;

        public UsuarioController(daoUsuariosAsync daoUsuarioP)
        {
            _DaoUsuarios = daoUsuarioP;
        }

        [HttpGet("version3")]
        public IActionResult GetVersion()
        {
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "No version";
            return Ok(new { version = apiVersion });
            // return Ok (new { version = apiVersion.ToString() });

        }

        [HttpGet("v3")]
        [MapToApiVersion("3.0")]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");

                var usuarios = await _DaoUsuarios.ObtenerUsuariosAsync();
                return Ok(new ApiResponse<List<Usuario>>(200, $"Usuarios obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", usuarios));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<Usuario>>(500, $"Error: {ex.Message}"));
            }

        }

        [HttpPost("v3")]
        [MapToApiVersion("3.0")]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {

           
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ApiResponse<object>(400, "Datos de usuario inválidos"));
            }

            if (string.IsNullOrWhiteSpace(usuario.UsuarioLg))
            {
                return BadRequest(new ApiResponse<object>(400, "El login del usuario es obligatorio"));
            }

            try
            {

                
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");

                await _DaoUsuarios.InsertarUsuarioAsync(usuario);
                return Ok(new ApiResponse<object>(201, $"Usuario creado correctamente.  Petición hecha por: {usuarioLogueado}"));

                
               


            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));

            }

        }

        [HttpPut("v3/{id}")]
        [MapToApiVersion("3.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(400, "Datos de usuario inválidos"));
            }
            if (string.IsNullOrWhiteSpace(usuario.UsuarioLg))
            {
                return BadRequest(new ApiResponse<object>(400, "El nombre de usuario es obligatorio"));
            }
            try
            {

               
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");
                usuario.IdUsuario = id;
                await _DaoUsuarios.ActualizarUsuarioAsync(usuario);
                return Ok(new ApiResponse<object>(200, $"Usuario actualizado correctamente. Petición hecha por: {usuarioLogueado}"));

               
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }
            catch
            {
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar usuario" });
            }

        }

        [HttpDelete("v3/{id}")]
        [MapToApiVersion("3.0")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");
                await _DaoUsuarios.EliminarUsuarioAsync(id);
                return Ok(new ApiResponse<object>(200, $"Usuario eliminado correctamente. Petición hecha por: {usuarioLogueado}"));
            }
            catch (NotFoundException nfex)
            {
                return NotFound(new ApiResponse<object>(404, nfex.Message));
            }

            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));
            }

        }


       
    }
}
