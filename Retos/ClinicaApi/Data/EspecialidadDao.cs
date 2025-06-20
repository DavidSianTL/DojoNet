using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClinicaApi.DAL;

public class EspecialidadDao
{
    private readonly string _connectionString;

    public EspecialidadDao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
    }

    public async Task<List<Especialidad>> ObtenerTodasAsync()
    {
        var lista = new List<Especialidad>();
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Especialidades", conn);
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Especialidad
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString()!
            });
        }

        return lista;
    }

    public async Task<Especialidad?> ObtenerPorIdAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Especialidades WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Especialidad
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString()!
            };
        }

        return null;
    }

    public async Task CrearAsync(Especialidad especialidad)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("INSERT INTO Especialidades (Nombre) VALUES (@Nombre)", conn);
        cmd.Parameters.AddWithValue("@Nombre", especialidad.Nombre);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(int id, Especialidad especialidad)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("UPDATE Especialidades SET Nombre = @Nombre WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Nombre", especialidad.Nombre);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("DELETE FROM Especialidades WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
