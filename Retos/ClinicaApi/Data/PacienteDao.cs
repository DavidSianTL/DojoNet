using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaApi.DAL;

public class PacienteDao
{
    private readonly string _connectionString;
    private readonly ILogger<PacienteDao> _logger;

    public PacienteDao(IConfiguration configuration, ILogger<PacienteDao> logger)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
        _logger = logger;
    }

    public async Task<List<Paciente>> ObtenerTodosAsync()
    {
        _logger.LogInformation("Obteniendo todos los pacientes");
        var lista = new List<Paciente>();
        try
        {
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los pacientes");
        }

        return lista;
    }

    public async Task<Paciente?> ObtenerPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando paciente con ID {Id}", id);
        try
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener paciente con ID {Id}", id);
        }

        return null;
    }

    public async Task CrearAsync(Paciente paciente)
    {
        _logger.LogInformation("Creando nuevo paciente: {Nombre}", paciente.Nombre);
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear paciente: {Nombre}", paciente.Nombre);
            throw;
        }
    }

    public async Task ActualizarAsync(int id, Paciente paciente)
    {
        _logger.LogInformation("Actualizando paciente ID {Id}", id);
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar paciente ID {Id}", id);
            throw;
        }
    }

    public async Task EliminarAsync(int id)
    {
        _logger.LogInformation("Eliminando paciente ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("DELETE FROM Pacientes WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar paciente ID {Id}", id);
            throw;
        }
    }
}
