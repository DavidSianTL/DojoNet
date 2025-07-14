using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiBanco.Services{

    public class JWTService{

        // Extraemos la configuración de JWT 
        private readonly IConfiguration _configuration;

        // Constructor que recibe la configuración para inyección de dependencias
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para generar un token JWT
        public string GenerarToken(int IdUsuario, string Usuario)
        {
            // Extraemos la configuración de JWT desde la sección "JwtSettings"
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Guardamos la clave secreta para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Clave"]));

            // Definimos la fecha de creación y la fecha de expiración del token
            var fechaCreacion = DateTime.UtcNow;

            // Calculamos la fecha de expiración sumando los minutos de expiración a la fecha de creación
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
                NotBefore = fechaCreacion, // El token es válido desde ahora
                Expires = fechaExpiracion,  // El token expira en X minutos
                IssuedAt = fechaCreacion,   // El token fue emitido ahora

                // Indicamos que el token es válido para la audiencia y el emisor especificados en la configuración
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
