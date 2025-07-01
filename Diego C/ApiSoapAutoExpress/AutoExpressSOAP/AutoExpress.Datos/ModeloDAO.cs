using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AutoExpress.Entidades;

public class ModeloDAO
{
    private readonly string connectionString;

    public ModeloDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Modelo ObtenerPorNombreYMarca(string nombre, int marcaId)
    {
        Modelo modelo = null;

        using (var conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, Nombre, MarcaId FROM Modelos WHERE Nombre = @Nombre AND MarcaId = @MarcaId";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@MarcaId", marcaId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        modelo = new Modelo
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            MarcaId = (int)reader["MarcaId"]
                        };
                    }
                }
            }
        }

        return modelo;
    }

    public int Agregar(Modelo modelo)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Modelos (Nombre, MarcaId) VALUES (@Nombre, @MarcaId); SELECT SCOPE_IDENTITY();";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", modelo.Nombre);
                cmd.Parameters.AddWithValue("@MarcaId", modelo.MarcaId);
                conn.Open();

                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }

    public List<Modelo> ListarPorMarca(int marcaId)
    {
        var lista = new List<Modelo>();

        using (var conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, Nombre, MarcaId FROM Modelos WHERE MarcaId = @MarcaId";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MarcaId", marcaId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Modelo
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            MarcaId = (int)reader["MarcaId"]
                        });
                    }
                }
            }
        }

        return lista;
    }
}
