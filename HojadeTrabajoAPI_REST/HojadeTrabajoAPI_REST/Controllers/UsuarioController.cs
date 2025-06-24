using HojadeTrabajoAPI_REST.DAO;
using HojadeTrabajoAPI_REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace HojadeTrabajoAPI_REST.Controllers
{
        [ApiController]
        [Route("api/v1/[controller]")]
        public class UsuarioController : ControllerBase
        {
            private readonly daoUsuarioAsync _daoUsuario;

            public UsuarioController(daoUsuarioAsync daoUsuario)
            {
                _daoUsuario = daoUsuario;
            }

            [HttpPost]
            public async Task<IActionResult> InsertarUsuario([FromBody] Usuario usuario)
            {
                await _daoUsuario.InsertarUsuarioAsync(usuario);
                return Ok(new { mensaje = "Usuario insertado correctamente" });
            }
        }

    }

