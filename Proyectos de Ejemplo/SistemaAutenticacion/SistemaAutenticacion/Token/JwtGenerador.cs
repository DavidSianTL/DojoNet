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
               new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!), //EJ. jose25
               new Claim("userId", usuario.Id), //EJ. 123abc
               new Claim("email", usuario.Email!) //EJ. jose@example.com
            };

            //Se crea la palabra clave y se codifica
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

            //Se encriptando la palabra clave
            var credencialesKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //Configurcion y firma del Token
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //HEADER
                Expires = DateTime.UtcNow.AddDays(1), //EXPIRACION
                SigningCredentials = credencialesKey, //SIGNATURE
            };

            //Se crea el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            //Se retorna el token como un string
            return tokenHandler.WriteToken(token);
        }

    }
}
