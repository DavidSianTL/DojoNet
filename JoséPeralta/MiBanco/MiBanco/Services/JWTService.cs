using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiBanco.Services{

    public class JWTService{

        // Extraemos la configuraci�n de JWT 
        private readonly IConfiguration _configuration;

        // Constructor que recibe la configuraci�n para inyecci�n de dependencias
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // M�todo para generar un token JWT
        public string GenerarToken(int IdUsuario, string Usuario)
        {
            // Extraemos la configuraci�n de JWT desde la secci�n "JwtSettings"
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Guardamos la clave secreta para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"]));

            // Definimos la fecha de creaci�n y la fecha de expiraci�n del token
            var fechaCreacion = DateTime.UtcNow;

            // Calculamos la fecha de expiraci�n sumando los minutos de expiraci�n a la fecha de creaci�n
            var fechaExpiracion = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DuracionEnMinutos"]));

            // Creamos el descriptor del token con la identidad del usuario y las credenciales de firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Definimos la identidad del usuario que contiene los claims
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("IdUsuario", IdUsuario.ToString()),
                    new Claim("Usuario", Usuario.ToString()),
                    
                }),

                // Periodo de validez del token
                NotBefore = fechaCreacion, // El token es v�lido desde ahora
                Expires = fechaExpiracion,  // El token expira en X minutos
                IssuedAt = fechaCreacion,   // El token fue emitido ahora

                // Indicamos que el token es v�lido para la audiencia y el emisor especificados en la configuraci�n
                Issuer = jwtSettings["Emisor"], // Emisor del token

                // Definimos la audiencia y el emisor del token
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            // Creamos el manejador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // Creamos el token usando el descriptor que hemos definido
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Devolvemos el token firmado
            return tokenHandler.WriteToken(token);

        }

    }

}
