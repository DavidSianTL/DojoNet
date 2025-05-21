using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Tokens
{

    public interface IJWTGenerator
    {
        string GenerateToken(UsuarioViewModel usuario);
    }

    // <summary>
    // Clase que genera el token JWT
    // </summary>
    public class JWTGenerator : IJWTGenerator
    {
        
        public string GenerateToken(UsuarioViewModel usuario)
        {
            // Crear los claims (los claims son los datos que se van a enviar en el token)
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
                new Claim("userId", usuario.Id), // Id del usuario
                new Claim("email", usuario.Email!) // Email del usuario
            };

            // Generar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PalabraSecreta"));

            // Crear la clave de firma
            var credentialsKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Formar el token
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Expira en 1 hora
                SigningCredentials = credentialsKey
            };

            // El tokenHandler es el encargado de crear el token y
            // firmarlo con la clave de firma
            var tokenHandler = new JwtSecurityTokenHandler();

            // Finalizar el token
            var token = tokenHandler.CreateToken(tokenDescription); 


            // Devolver el token
            return tokenHandler.WriteToken(token);

        }

    }

}
