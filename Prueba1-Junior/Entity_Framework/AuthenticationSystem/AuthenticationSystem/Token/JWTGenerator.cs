using AuthenticationSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationSystem.Token
{

	public interface IJWTGenerator
	{
		string GenerateToken(Usuario user);
	}


	public class JWTGenerator : IJWTGenerator
	{
		public string GenerateToken(Usuario user)
		{
			// Crear claims para el Json Web Token con los datos del usuario
			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),
				new Claim("userId", user.Id),
				new Claim("email", user.Email!)
			};

			// Se crea la palabra clave y se codifica
			var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("$EstaEsmiClaveSeguraDe32Bytesxd!"));

			// Se encripta la palabra clave ya codificada
			var EncryptedSecurityKey = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

			// Se crea la configuración del token y se firma
			var TokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims), // Header (datos del usuario)
				Expires = DateTime.UtcNow.AddDays(1), // Exoira en 1 día 
				SigningCredentials = EncryptedSecurityKey // Firma del token
			};
			// Finalmente se crea el token con la configuración
			var TokenHandler = new JwtSecurityTokenHandler();
			var token = TokenHandler.CreateToken(TokenDescriptor);

			// Se retorna el token como string
			return TokenHandler.WriteToken(token);
		}
	}
}
