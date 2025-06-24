using Asp.Versioning;
using HojadeTrabajoAPI_REST.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using HojadeTrabajoAPI_REST.DAO;
using HojadeTrabajoAPI_REST.Models;


namespace HojadeTrabajoAPI_REST.Controllers.v1
{
        [ApiVersion("1.0")]
        [Route("api/v{version:ApiVersion}/[controller]")]
        [ApiController]
        public class AutorizacionController : Controller
        {
            private readonly daoUsuarioAsync _daoUsuarioAsync;
            private readonly JwtService _jwtService;
            private readonly ILogger<AutorizacionController> _logger;

            public AutorizacionController(daoUsuarioAsync daoUsuario, JwtService jwtService, ILogger<AutorizacionController> logger)
            {
                _daoUsuarioAsync = daoUsuario;
                _jwtService = jwtService;
                _logger = logger;
            }

            [HttpPost("login")]
            [MapToApiVersion("1.0")]
            public async Task<IActionResult> Login([FromBody] HojadeTrabajoAPI_REST.Models.LoginRequest request)
            {
                var usuario = await _daoUsuarioAsync.ObtenerUsuarioAsync(request.Usuario);
                var usuarioLogueado = User.Identity?.Name ?? "desconocido";

                if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
                {
                    _logger.LogError("401 - Credenciales incorrectas");
                    return Unauthorized(new { mensaje = "Credenciales incorrectas" });
                }

                var token = _jwtService.GenerateToken(usuario.UsuarioS);
                _logger.LogInformation("Petición LOGIN usuarios hecha por: {Usuario}", usuarioLogueado);

                return Ok(new LoginResponse
                {
                    Token = token,
                    Usuario = usuario.UsuarioS
                });
            }
        }
    }

