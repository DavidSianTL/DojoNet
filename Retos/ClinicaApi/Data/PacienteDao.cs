using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClinicaApi.DAL;

public class PacienteDao
{
    private readonly string _connectionString;

    public PacienteDao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
    }

    public async Task<List<Paciente>> ObtenerTodosAsync()
    {
        var lista = new List<Paciente>();
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Pacientes", conn);
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Paciente
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString()!,
                Email = reader["Email"].ToString()!,
                Telefono = reader["Telefono"].ToString()!,
                FechaNacimiento = (DateTime)reader["FechaNacimiento"]
            });
        }

        return lista;
    }

    public async Task<Paciente?> ObtenerPorIdAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Pacientes WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Paciente
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString()!,
                Email = reader["Email"].ToString()!,
                Telefono = reader["Telefono"].ToString()!,
                FechaNacimiento = (DateTime)reader["FechaNacimiento"]
            };
        }
        return null;
    }

    public async Task CrearAsync(Paciente paciente)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(
            "INSERT INTO Pacientes (Nombre, Email, Telefono, FechaNacimiento) " +
            "VALUES (@Nombre, @Email, @Telefono, @FechaNacimiento)", conn);
        cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
        cmd.Parameters.AddWithValue("@Email", paciente.Email);
        cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
        cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ActualizarAsync(int id, Paciente paciente)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(
            "UPDATE Pacientes SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono, FechaNacimiento = @FechaNacimiento " +
            "WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
        cmd.Parameters.AddWithValue("@Email", paciente.Email);
        cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
        cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("DELETE FROM Pacientes WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
