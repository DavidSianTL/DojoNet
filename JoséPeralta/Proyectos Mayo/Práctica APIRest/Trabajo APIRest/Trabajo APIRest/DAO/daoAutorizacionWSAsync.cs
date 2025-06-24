using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trabajo_APIRest.Models;
using Trabajo_APIRest.Data;
using Trabajo_APIRest.Dtos.UsuarioDtos;

namespace Trabajo_APIRest.DAO
{
    public class daoAutorizacionWSAsync
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public daoAutorizacionWSAsync(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Valida usuario y contraseña, devuelve UsuarioViewModel con token si es correcto
        public async Task<UsuarioResponseDto> LoginAsync(UsuarioLoginRequestDto request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Usuario == request.Usuario && u.Contrasenia == request.Contrasenia);
            if (usuario == null)
                return null;

            var token = GenerarToken(usuario);
            usuario.Token = token;
            await _context.SaveChangesAsync();

            return new UsuarioResponseDto
            {
                Usuario = usuario.Usuario,
                Token = token
            };
        }

        // Genera el JWT
        private string GenerarToken(UsuarioViewModel usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"]));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Usuario),
                new Claim("id", usuario.IdUsuario.ToString())
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(60);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Usuario"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
