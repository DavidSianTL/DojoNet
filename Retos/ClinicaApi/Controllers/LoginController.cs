using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClinicaApi.DAL;
using ClinicaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClinicaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly UsuarioDao _dao;
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _dao = new UsuarioDao(config);
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _dao.ObtenerPorUsernameAsync(request.Username);

        if (usuario == null)
        {
            return Unauthorized(new ApiResponse("401", "Usuario no encontrado"));
        }

        bool valido = BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password);

        if (!valido)
        {
            return Unauthorized(new ApiResponse("401", "Credenciales incorrectas"));
        }

        var token = GenerarJwtToken(usuario);

        return Ok(new ApiResponse("200", "Login exitoso", new { token }));
    }

    private string GenerarJwtToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            // Si agregás roles:
            // new Claim(ClaimTypes.Role, usuario.Rol)
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
