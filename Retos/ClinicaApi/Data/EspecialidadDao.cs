using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaApi.DAL;

public class EspecialidadDao
{
    private readonly string _connectionString;
    private readonly ILogger<EspecialidadDao> _logger;

    public EspecialidadDao(IConfiguration configuration, ILogger<EspecialidadDao> logger)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
        _logger = logger;
    }

    public async Task<List<Especialidad>> ObtenerTodasAsync()
    {
        _logger.LogInformation("Obteniendo todas las especialidades");
        var lista = new List<Especialidad>();
        try
        {
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener especialidades");
        }

        return lista;
    }

    public async Task<Especialidad?> ObtenerPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando especialidad con ID {Id}", id);
        try
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener especialidad con ID {Id}", id);
        }

        return null;
    }

    public async Task CrearAsync(Especialidad especialidad)
    {
        _logger.LogInformation("Creando nueva especialidad: {Nombre}", especialidad.Nombre);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("INSERT INTO Especialidades (Nombre) VALUES (@Nombre)", conn);
            cmd.Parameters.AddWithValue("@Nombre", especialidad.Nombre);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear especialidad: {Nombre}", especialidad.Nombre);
            throw;
        }
    }

    public async Task ActualizarAsync(int id, Especialidad especialidad)
    {
        _logger.LogInformation("Actualizando especialidad ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("UPDATE Especialidades SET Nombre = @Nombre WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Nombre", especialidad.Nombre);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar especialidad ID {Id}", id);
            throw;
        }
    }

    public async Task EliminarAsync(int id)
    {
        _logger.LogInformation("Eliminando especialidad ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("DELETE FROM Especialidades WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar especialidad ID {Id}", id);
            throw;
        }
    }
}
