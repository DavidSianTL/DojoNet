using AutoExpress.Entidades;
using System.Collections.Generic;
using System.Data.SqlClient;

public class CarroDAO
{
    private readonly string connectionString;

    public CarroDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<Carro> ListarCarros()
    {
        var lista = new List<Carro>();

        using (var connection = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT c.Id, c.Año, c.Precio, c.Disponible,
                       m.Id AS ModeloId, m.Nombre AS ModeloNombre,
                       ma.Id AS MarcaId, ma.Nombre AS MarcaNombre
                FROM Carros c
                JOIN Modelos m ON c.ModeloId = m.Id
                JOIN Marcas ma ON m.MarcaId = ma.Id";

            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Carro
                        {
                            Id = (int)reader["Id"],
                            Año = (int)reader["Año"],
                            Precio = (decimal)reader["Precio"],
                            Disponible = (bool)reader["Disponible"],
                            ModeloId = (int)reader["ModeloId"],
                            Modelo = new Modelo
                            {
                                Id = (int)reader["ModeloId"],
                                Nombre = reader["ModeloNombre"].ToString(),
                                MarcaId = (int)reader["MarcaId"],
                                Marca = new Marca
                                {
                                    Id = (int)reader["MarcaId"],
                                    Nombre = reader["MarcaNombre"].ToString()
                                }
                            }
                        });
                    }
                }
            }
        }

        return lista;
    }
    public void AgregarCarro(Carro carro)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO Carros (ModeloId, Año, Precio, Disponible)
                         VALUES (@ModeloId, @Año, @Precio, @Disponible)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ModeloId", carro.ModeloId);
                command.Parameters.AddWithValue("@Año", carro.Año);
                command.Parameters.AddWithValue("@Precio", carro.Precio);
                command.Parameters.AddWithValue("@Disponible", carro.Disponible);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public void EditarCarro(Carro carro)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string query = @"UPDATE Carros SET ModeloId=@ModeloId, Año=@Año, Precio=@Precio, Disponible=@Disponible
                         WHERE Id=@Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ModeloId", carro.ModeloId);
                command.Parameters.AddWithValue("@Año", carro.Año);
                command.Parameters.AddWithValue("@Precio", carro.Precio);
                command.Parameters.AddWithValue("@Disponible", carro.Disponible);
                command.Parameters.AddWithValue("@Id", carro.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public void EliminarCarro(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE Carros SET Disponible = 0 WHERE Id = @Id";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}

