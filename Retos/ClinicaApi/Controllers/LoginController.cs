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
    public async Task<Usuario?> ValidarCredencialesAsync(string username, string passwordPlano)
    {
        var usuario = await ObtenerPorUsernameAsync(username);

        if (usuario == null)
            return null;

        bool esValida = BCrypt.Net.BCrypt.Verify(passwordPlano, usuario.Password);
        return esValida ? usuario : null;
    }

    var token = GenerarJwtToken(usuario);
        return Ok(new
        {
            ResponseCode = "200",
            ResponseMessage = "Login exitoso",
            token = token
        });
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
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
