
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Models;
using UsuariosApi.Services;
using UsuariosApi.DAO;
using Asp.Versioning;


namespace UsuariosApi.Controllers.v4
{
    [ApiVersion("4.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class AutorizacionController : Controller
    {
        private readonly daoUsuariosAsync _daoUsuarioAsync;
        private readonly JwtService _jwtService;
        private readonly ILogger<AutorizacionController> _logger;

        public AutorizacionController(daoUsuariosAsync daoUsuario, JwtService jwtService, ILogger<AutorizacionController> logger)
        {
            _daoUsuarioAsync = daoUsuario;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        [MapToApiVersion("4.0")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _daoUsuarioAsync.ObtenerUsuarioPorNombreAsync(request.Usuario);
            var usuarioLogueado = User.Identity?.Name ?? "desconocido";

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasenia, usuario.Contrasenia))
            {
                _logger.LogError("401 - Credenciales incorrectas");
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            var token = _jwtService.GenerateToken(usuario.UsuarioLg);
            _logger.LogInformation("Petición LOGIN usuarios hecha por: {Usuario}", usuarioLogueado);
         
            return Ok(new LoginResponse
            {
                Token = token,
                Usuario = usuario.UsuarioLg
            });
        }
    }
}
