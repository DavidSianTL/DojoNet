using Microsoft.IdentityModel.Tokens;
using SistemaAutenticacion.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaAutenticacion.Token
{
    public interface IJwtGenerador
    {
        string GenerarToken(Usuarios usuario);
    }

    /// <summary>
    /// Funcion la cual permite crear o genera tokens
    /// </summary>
    public class JwtGenerador: IJwtGenerador
    {
        public string GenerarToken(Usuarios usuario)
        {
            var claims = new List<Claim>()
            {
               new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
               new Claim("userId", usuario.Id),
               new Claim("email", usuario.Email!)
            };

            //Se crea la palabra clave y se codifica
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

            //Se encriptando la palabra clave
            var credencialesKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //Configurcion y firma del Token
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credencialesKey,
            };

            //Se crea el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            //Se retorna el token como un string
            return tokenHandler.WriteToken(token);
        }

    }
}
