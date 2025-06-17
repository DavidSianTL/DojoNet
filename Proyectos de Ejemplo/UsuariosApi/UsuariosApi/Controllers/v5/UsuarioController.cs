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
namespace UsuariosApi.Controllers.v5

{
    [ApiVersion("5.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly daoUsuarioAsyncEF _DaoUsuariosAsync;

        public UsuarioController(daoUsuarioAsyncEF daoUsuarioP)
        {
            _DaoUsuariosAsync = daoUsuarioP;
        }

        [HttpGet("version5")]
        public IActionResult GetVersion()
        {
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "No version";
            return Ok(new { version = apiVersion });
            // return Ok (new { version = apiVersion.ToString() });

        }
        [Authorize]
        [HttpGet("v5")]
        [MapToApiVersion("5.0")]
        public async Task<ActionResult<List<UsuarioEF>>> Get()
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";
                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");

                var usuarios = await _DaoUsuariosAsync.ObtenerUsuariosAsync();
                return Ok(new ApiResponse<List<UsuarioEF>>(200, $"Usuarios obtenidos correctamente.  Petición hecha por: {usuarioLogueado}", usuarios));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<UsuarioEF>>(500, $"Error: {ex.Message}"));
            }

        }

        [HttpPost("v5")]
        [MapToApiVersion("5.0")]
        public async Task<IActionResult> Post([FromBody] UsuarioEF usuario)
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

                await _DaoUsuariosAsync.InsertarUsuarioAsync(usuario);
                return Ok(new ApiResponse<object>(201, $"Usuario creado correctamente.  Petición hecha por: {usuarioLogueado}"));

                
               


            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, $"Error: {ex.Message}"));

            }

        }

        [HttpPut("v5/{id}")]
        [MapToApiVersion("5.0")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioEF usuario)
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
                await _DaoUsuariosAsync.ActualizarUsuarioAsync(usuario);
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

        [HttpDelete("v5/{id}")]
        [MapToApiVersion("5.0")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                // Por ejemplo, loguear en consola
                Console.WriteLine($"Petición GET usuarios hecha por: {usuarioLogueado}");
                await _DaoUsuariosAsync.EliminarUsuarioAsync(id);
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
