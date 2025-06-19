using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UsuariosApi.DAO;
using UsuariosApi.Models;
using Asp.Versioning;

namespace UsuariosApi.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly daoUsuariosAsync _DaoUsuarios;

        public UsuarioController(daoUsuariosAsync daoUsuarioP)
        {
            _DaoUsuarios = daoUsuarioP;
        }
        [HttpGet("version2")]
        public IActionResult GetVersion()
        {
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "No version";
            return Ok(new { version = apiVersion });
         

        }

        [HttpGet("v2")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            try
            {
                var usuarios = await _DaoUsuarios.ObtenerUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "ERROR: Al obtener usuarios. ", detalle = ex.Message });
            }

        }

        [HttpPost("v2")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {

            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(new { code = 400, mensaje = "Datos de usuario son invalidos" });
                }
                if (string.IsNullOrWhiteSpace(usuario.UsuarioLg))
                {
                    return BadRequest(new { code = 400, mensaje = "El login del usuario no  puede ser vacio o nulo" });
                }
                
                
                if (ModelState.IsValid)
                {
                    await _DaoUsuarios.InsertarUsuarioAsync(usuario);
                    return Ok(new { mensaje = "Usuario creado exitosamente." });

                }
                else
                {
                    return BadRequest(ModelState);

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "ERROR: Error al crear usuario.", detalle = ex.Message });

            }

        }

        [HttpPut("v2/{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuario.IdUsuario = id;
                    await _DaoUsuarios.ActualizarUsuarioAsync(usuario);
                    return Ok(new { mensaje = "Usuario actualizado correctamente." });

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch
            {
                return StatusCode(500, new { mensaje = "ERROR: Al actualizar usuario" });
            }

        }

        [HttpDelete("v2/{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _DaoUsuarios.EliminarUsuarioAsync(id);
                return Ok(new { mensaje = "Usuario eliminado exitosamente." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "ERROR: Al eliminar usuario" });
            }

        }


       
    }
}
