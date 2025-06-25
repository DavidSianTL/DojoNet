using ClinicaMedicaAPIREST.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClinicaMedicaAPIREST.Security
{
    public interface IJwtGenerator
    {
        string GenerateToken(Usuario usuario);

    }


    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var SecurityKey = _configuration["Jwt:Key"];

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecurityKey!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expiration = _configuration["Jwt:DurationInMinutes"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiration!)),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
