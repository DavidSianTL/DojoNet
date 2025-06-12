using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using wbSistemaSeguridadMVC.Models;

namespace wbSistemaSeguridadMVC.Data
{
    public class daoUsuarioT
    {
        private readonly string _connectionString;

        public daoUsuarioT(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UsuarioT ValidarUsuario(string usuario, string claveIngresada)
        {
            UsuarioT user = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT id_usuario, usuario, nom_usuario, contrasenia, fk_id_estado
                    FROM Usuarios
                    WHERE usuario = @usuario AND fk_id_estado = 1", conn);

                cmd.Parameters.AddWithValue("@usuario", usuario);
                

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashGuardado = reader["contrasenia"].ToString();

                        if (BCrypt.Net.BCrypt.Verify(claveIngresada, hashGuardado))
                        {
                            user = new UsuarioT
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                UsuarioLogin = reader["usuario"].ToString(),
                                NombreCompleto = reader["nom_usuario"].ToString(),
                                Contrasenia = reader["contrasenia"].ToString(),
                                Estado = Convert.ToInt32(reader["fk_id_estado"])
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
