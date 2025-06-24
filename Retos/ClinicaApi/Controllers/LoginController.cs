using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly UsuarioDao _dao;
    private readonly IConfiguration _config;
    private readonly ILogger<LoginController> _logger;

    public LoginController(UsuarioDao dao, IConfiguration config, ILogger<LoginController> logger)
    {
    _dao = dao;
    _config = config;
    _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Intentando login para el usuario: {Username}", request.Username);

        var usuario = await _dao.ObtenerPorUsernameAsync(request.Username);

        if (usuario == null)
        {
            _logger.LogWarning("Usuario no encontrado: {Username}", request.Username);
            return Unauthorized(new ApiResponse("401", "Usuario no encontrado"));
        }

        bool valido = BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password);

        if (!valido)
        {
            _logger.LogWarning("Contraseña incorrecta para usuario: {Username}", request.Username);
            return Unauthorized(new ApiResponse("401", "Credenciales incorrectas"));
        }

        var token = GenerarJwtToken(usuario);
        _logger.LogInformation("Login exitoso para el usuario: {Username}", request.Username);

        return Ok(new ApiResponse("200", "Login exitoso", new { token }));
    }

    private string GenerarJwtToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
