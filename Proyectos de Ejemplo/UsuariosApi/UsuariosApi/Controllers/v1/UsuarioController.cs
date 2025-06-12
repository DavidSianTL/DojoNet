using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using UsuariosApi.DAO;
using UsuariosApi.Models;

namespace UsuariosApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly daoUsuarios _DaoUsuarios;

        public UsuarioController(daoUsuarios daoUsuarioP)
        {
            _DaoUsuarios = daoUsuarioP;
        }
        [HttpGet("version1")]
        public IActionResult GetVersion()
        {
            var apiVersion = HttpContext.GetRequestedApiVersion()?.ToString() ?? "No version";
            return Ok(new { version = apiVersion });
           // return Ok (new { version = apiVersion.ToString() });
        
        }
        


        [HttpGet("v1")]
        [MapToApiVersion("1.0")]
        public ActionResult<List<Usuario>> Get()
        {
            try
            {
                var usuarios = _DaoUsuarios.ObtenerUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "ERROR: Al obtener usuarios. ", detalle = ex.Message });
            }

        }

        [HttpPost("v1")]
        [MapToApiVersion("1.0")]
        public IActionResult Post([FromBody] Usuario usuario)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _DaoUsuarios.InsertarUsuario(usuario);
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

        [HttpPut("v1/{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuario.IdUsuario = id;
                    _DaoUsuarios.ActualizarUsuario(usuario);
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

        [HttpDelete("v1/{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Delete(int id)
        {
            try
            {
                _DaoUsuarios.EliminarUsuario(id);
                return Ok(new { mensaje = "Usuario eliminado exitosamente." });
            }
            catch
            {
                return StatusCode(500, new { mensaje = "ERROR: Al eliminar usuario" });
            }

        }


       
    }
}
