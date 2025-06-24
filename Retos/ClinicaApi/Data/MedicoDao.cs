using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClinicaApi.DAL;

public class MedicoDao
{
    private readonly string _connectionString;

    public MedicoDao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
    }

    public async Task<List<Medico>> ObtenerTodosAsync()
    {
        var lista = new List<Medico>();
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Medicos", conn);
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new Medico
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString()!,
                Email = reader["Email"].ToString()!,
                EspecialidadId = (int)reader["EspecialidadId"]
            });
        }

        return lista;
    }

  public async Task CrearAsync(Medico medico)
    {   
    using SqlConnection conn = new(_connectionString);
    using SqlCommand cmd = new(
        "INSERT INTO Medicos (Nombre, Email, EspecialidadId, Telefono) " +
        "VALUES (@Nombre, @Email, @EspecialidadId, @Telefono)", conn);

    cmd.Parameters.AddWithValue("@Nombre", medico.Nombre);
    cmd.Parameters.AddWithValue("@Email", medico.Email);
    cmd.Parameters.AddWithValue("@EspecialidadId", medico.EspecialidadId);
    cmd.Parameters.AddWithValue("@Telefono", medico.Telefono); // ← ¡Este es el que faltaba!

    await conn.OpenAsync();
    await cmd.ExecuteNonQueryAsync();
    }


    public async Task ActualizarAsync(int id, Medico medico)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new(
            "UPDATE Medicos SET Nombre = @Nombre, Email = @Email, EspecialidadId = @EspecialidadId WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Nombre", medico.Nombre);
        cmd.Parameters.AddWithValue("@Email", medico.Email);
        cmd.Parameters.AddWithValue("@EspecialidadId", medico.EspecialidadId);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("DELETE FROM Medicos WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<Medico?> ObtenerPorIdAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Medicos WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Medico
            {
                Id = (int)reader["Id"],
                Nombre = reader["Nombre"].ToString()!,
                Email = reader["Email"].ToString()!,
                EspecialidadId = (int)reader["EspecialidadId"]
            };
        }
        return null;
    }
}
