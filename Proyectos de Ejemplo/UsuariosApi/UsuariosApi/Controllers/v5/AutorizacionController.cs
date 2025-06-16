
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Models;
using UsuariosApi.Services;
using UsuariosApi.DAO;
using Asp.Versioning;


namespace UsuariosApi.Controllers.v5
{
    [ApiVersion("5.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class AutorizacionController : Controller
    {
        private readonly daoUsuarioAsyncEF _daoUsuarioAsync;
        private readonly JwtService _jwtService;

        public AutorizacionController(daoUsuarioAsyncEF daoUsuario, JwtService jwtService)
        {
            _daoUsuarioAsync = daoUsuario;
            _jwtService = jwtService;
        }

        [HttpPost("login/v5")]
        [MapToApiVersion("5.0")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _daoUsuarioAsync.ObtenerUsuarioPorNombreAsync(request.Usuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasenia, usuario.Contrasenia))
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            var token = _jwtService.GenerateToken(usuario.UsuarioLg);

            return Ok(new LoginResponse
            {
                Token = token,
                Usuario = usuario.UsuarioLg
            });
        }
    }
}
