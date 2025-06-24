using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaApi.DAL;

public class MedicoDao
{
    private readonly string _connectionString;
    private readonly ILogger<MedicoDao> _logger;

    public MedicoDao(IConfiguration configuration, ILogger<MedicoDao> logger)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
        _logger = logger;
    }

    public async Task<List<Medico>> ObtenerTodosAsync()
    {
        _logger.LogInformation("Obteniendo todos los médicos");
        var lista = new List<Medico>();
        try
        {
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los médicos");
        }

        return lista;
    }

    public async Task<Medico?> ObtenerPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando médico con ID {Id}", id);
        try
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener médico con ID {Id}", id);
        }

        return null;
    }

    public async Task CrearAsync(Medico medico)
    {
        _logger.LogInformation("Creando nuevo médico: {Nombre}", medico.Nombre);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(
                "INSERT INTO Medicos (Nombre, Email, EspecialidadId, Telefono) " +
                "VALUES (@Nombre, @Email, @EspecialidadId, @Telefono)", conn);

            cmd.Parameters.AddWithValue("@Nombre", medico.Nombre);
            cmd.Parameters.AddWithValue("@Email", medico.Email);
            cmd.Parameters.AddWithValue("@EspecialidadId", medico.EspecialidadId);
            cmd.Parameters.AddWithValue("@Telefono", medico.Telefono);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear médico: {Nombre}", medico.Nombre);
            throw;
        }
    }

    public async Task ActualizarAsync(int id, Medico medico)
    {
        _logger.LogInformation("Actualizando médico ID {Id}", id);
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar médico ID {Id}", id);
            throw;
        }
    }

    public async Task EliminarAsync(int id)
    {
        _logger.LogInformation("Eliminando médico ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("DELETE FROM Medicos WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar médico ID {Id}", id);
            throw;
        }
    }
}
