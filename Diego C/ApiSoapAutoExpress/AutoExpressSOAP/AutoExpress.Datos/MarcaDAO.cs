using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AutoExpress.Entidades;

public class MarcaDAO
{
    private readonly string connectionString;

    public MarcaDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Marca ObtenerPorNombre(string nombre)
    {
        Marca marca = null;

        using (var conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, Nombre FROM Marcas WHERE Nombre = @Nombre";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        marca = new Marca
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString()
                        };
                    }
                }
            }
        }

        return marca;
    }

    public int Agregar(Marca marca)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Marcas (Nombre) VALUES (@Nombre); SELECT SCOPE_IDENTITY();";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", marca.Nombre);
                conn.Open();

                // Ejecuta y devuelve el Id generado
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }

    public List<Marca> Listar()
    {
        var lista = new List<Marca>();

        using (var conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, Nombre FROM Marcas";
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Marca
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                }
            }
        }

        return lista;
    }
}
