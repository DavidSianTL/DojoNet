using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ServicioEnvioCorreos.Models;

namespace ServicioEnvioCorreos.DAO
{
    public class DAOCorreo
    {
        private readonly string _cadenaConexion;

        public DAOCorreo(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public List<Correo> ObtenerCorreosPendientes()
        {
            var lista = new List<Correo>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM CorreosPendientes WHERE Estado = 'Pendiente'", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Correo
                    {
                        Id = (int)reader["Id"],
                        Destinatario = reader["Destinatario"].ToString(),
                        CC = reader["CC"].ToString(),
                        Asunto = reader["Asunto"].ToString(),
                        Cuerpo = reader["Cuerpo"].ToString(),
                        RutaAdjunto = reader["RutaAdjunto"].ToString()
                    });
                }
            }

            return lista;
        }

        public void ActualizarEstado(int id, string estado)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE CorreosPendientes SET Estado = @Estado WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
