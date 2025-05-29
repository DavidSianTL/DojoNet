
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Helper
{
    // Clase JwtHelper que se encarga de generar tokens JWT para los usuarios
    public class JwtHelper
    {
        // Declaración de nuestra clave secreta y otros parámetros
        private readonly string secretKey = "3[W3(0R3QV1?0";
        // Tiempo de expiración del token en minutos
        private readonly int expirationMinutes = 20;

        // Constructor de la clase JwtHelper
        public TokenUsuarioViewModel GenerarToken(int IdUsuario, string UsrName)
        {
            // Creamos un manejador de tokens JWT y convertimos la clave secreta a bytes
            var tokenHandler = new JwtSecurityTokenHandler();
            // Convertimos la clave secreta a bytes usando UTF8
            var key = Encoding.UTF8.GetBytes(secretKey);

            // Definimos la fecha de creación y la fecha de expiración del token
            var fechaCreacion = DateTime.Now;
            // Calculamos la fecha de expiración sumando los minutos de expiración a la fecha de creación
            var fechaExpiracion = fechaCreacion.AddMinutes(expirationMinutes);

            // Creamos el descriptor del token con la identidad del usuario y las credenciales de firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Definimos la identidad del usuario que contiene los claims
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, UsrName),
                    new Claim("IdUsuario", IdUsuario.ToString()),
                }),
                // Establecemos la fecha de creación y expiración del token
                Expires = DateTime.Now.AddMinutes(expirationMinutes),
                // Definimos la audiencia y el emisor del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Creamos el token usando el manejador de tokens y el descriptor del token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Convertimos el token a una cadena de texto
            var valorToken = tokenHandler.WriteToken(token);

            // Creamos y retornamos un nuevo TokenUsuarioViewModel con los datos del token generado
            return new TokenUsuarioViewModel
            {
                FK_IdUsuario = IdUsuario,
                Token = valorToken,
                FechaCreacion = fechaCreacion,
                TiempoExpira = fechaExpiracion
            };

        }


    }
}
