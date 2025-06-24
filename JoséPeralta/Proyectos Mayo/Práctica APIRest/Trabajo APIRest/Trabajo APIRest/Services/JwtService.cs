
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string username)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"]));


            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                // Usamos ClaimTypes.NameIdentifier para el ID del usuario (opcional)
                new Claim(ClaimTypes.NameIdentifier, username),
                // Usamos ClaimTypes.Name para el nombre de usuario (asegura que User.Identity.Name funcione)
                new Claim(ClaimTypes.Name, username),
                // JWT ID único para el token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Subject claim (puede ser útil para compatibilidad)
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Usuario"],
                audience: jwtSettings["Sesion"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DuracionEnMinutos"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
