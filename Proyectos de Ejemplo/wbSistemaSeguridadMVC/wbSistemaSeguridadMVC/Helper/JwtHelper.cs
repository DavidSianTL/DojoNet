
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;

using wbSistemaSeguridadMVC.Data;
using wbSistemaSeguridadMVC.Models;


namespace wbSistemaSeguridadMVC.Helper
{
    public class JwtHelper
    {
        private readonly daoUsuario _datos;
        private readonly string secretKey = "3st3{urs03sWVyF@c1lP@r@Tr@b@74rl0!!M39vt4";
                                            //"3st3{urs03sWVyF@c1lP@r@Tr@b@74rl0!!M39vt4"
        private readonly int expirationMinutes = 30;
        private readonly string connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";

        public Token GenerarToken(string nombreUsuario, int idUsuario, int idSistema)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var fechaCreacion = DateTime.UtcNow;
            var fechaExpiracion = fechaCreacion.AddMinutes(expirationMinutes);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, nombreUsuario),
                new Claim("idUsuario", idUsuario.ToString()),
                new Claim("idSistema", idSistema.ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var valorToken = tokenHandler.WriteToken(token);


            return new Token
            {
                IdUsuario = idUsuario,
                IdSistema = idSistema,
                TokenS = valorToken,
                FechaCreacion = fechaCreacion,
                FechaExpiracion = fechaExpiracion,
                Estado = 1
            };
        }

        //private void GuardarTokenBD(string token, int idUsuario, int idSistema, DateTime fechaExpiracion)
        //{
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        var cmd = new SqlCommand(@"
        //        INSERT INTO Tokens (IdUsuario, IdSistema, Token, FechaCreacion, FechaExpiracion, Estado)
        //        VALUES (@idUsuario, @idSistema, @token, @fechaCreacion, @fechaExpiracion, 1)", conn);

        //        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
        //        cmd.Parameters.AddWithValue("@idSistema", idSistema);
        //        cmd.Parameters.AddWithValue("@token", token);
        //        cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.UtcNow);
        //        cmd.Parameters.AddWithValue("@fechaExpiracion", fechaExpiracion);

        //        cmd.ExecuteNonQuery();
        //    }
        //}

    }
}
