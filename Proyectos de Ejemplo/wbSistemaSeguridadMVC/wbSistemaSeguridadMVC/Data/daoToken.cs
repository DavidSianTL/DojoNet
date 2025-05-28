using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using wbSistemaSeguridadMVC.Models;


namespace wbSistemaSeguridadMVC.Data
{
    public class daoToken
    {
        private readonly string _connectionString;
        public daoToken(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void GuardarToken(Token token)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(@"
                    INSERT INTO Tokens 
                        (IdUsuario, IdSistema, Token, FechaCreacion, FechaExpiracion, Estado)
                    VALUES 
                        (@IdUsuario, @IdSistema, @Token, @FechaCreacion, @FechaExpiracion, @Estado)", conn);

                cmd.Parameters.AddWithValue("@IdUsuario", token.IdUsuario);
                cmd.Parameters.AddWithValue("@IdSistema", token.IdSistema);
                cmd.Parameters.AddWithValue("@Token", token.TokenS);
                cmd.Parameters.AddWithValue("@FechaCreacion", token.FechaCreacion);
                cmd.Parameters.AddWithValue("@FechaExpiracion", token.FechaExpiracion);
                cmd.Parameters.AddWithValue("@Estado", token.Estado);

                cmd.ExecuteNonQuery();
            }
        }

        public bool TokenEsValido(string valorToken)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("sp_ValidarToken", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", valorToken);

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
        }

        public void RevocarToken(string valorToken)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("sp_RevocarToken", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", valorToken);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
