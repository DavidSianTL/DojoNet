using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using UsuariosApi.DAO;
using UsuariosApi.Models;

namespace UsuariosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly daoUsuarios _DaoUsuarios;

        public UsuarioController(daoUsuarios daoUsuarioP)
        {
            _DaoUsuarios = daoUsuarioP;
        }
        [HttpGet]
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

        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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
