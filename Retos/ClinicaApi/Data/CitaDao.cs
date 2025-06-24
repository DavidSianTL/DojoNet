using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaApi.DAL;

public class CitaDao
{
    private readonly string _connectionString;
    private readonly ILogger<CitaDao> _logger;

    public CitaDao(IConfiguration configuration, ILogger<CitaDao> logger)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
        _logger = logger;
    }

    public async Task<List<Cita>> ObtenerTodasAsync()
    {
        _logger.LogInformation("Obteniendo todas las citas desde la base de datos");
        var lista = new List<Cita>();
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("SELECT * FROM Citas", conn);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Cita
                {
                    Id = (int)reader["Id"],
                    PacienteId = (int)reader["PacienteId"],
                    MedicoId = (int)reader["MedicoId"],
                    Fecha = (DateTime)reader["Fecha"],
                    Hora = ((TimeSpan)reader["Hora"]).ToString(@"hh\:mm")
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las citas");
        }

        return lista;
    }

    public async Task<Cita?> ObtenerPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando cita con ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("SELECT * FROM Citas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Cita
                {
                    Id = (int)reader["Id"],
                    PacienteId = (int)reader["PacienteId"],
                    MedicoId = (int)reader["MedicoId"],
                    Fecha = (DateTime)reader["Fecha"],
                    Hora = ((TimeSpan)reader["Hora"]).ToString(@"hh\:mm")
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la cita con ID {Id}", id);
        }

        return null;
    }

    public async Task CrearAsync(Cita cita)
    {
        _logger.LogInformation("Creando nueva cita para paciente ID {PacienteId}", cita.PacienteId);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(
                "INSERT INTO Citas (PacienteId, MedicoId, Fecha, Hora) VALUES (@PacienteId, @MedicoId, @Fecha, @Hora)", conn);
            cmd.Parameters.AddWithValue("@PacienteId", cita.PacienteId);
            cmd.Parameters.AddWithValue("@MedicoId", cita.MedicoId);
            cmd.Parameters.AddWithValue("@Fecha", cita.Fecha);
            cmd.Parameters.AddWithValue("@Hora", cita.Hora);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cita para paciente ID {PacienteId}", cita.PacienteId);
            throw;
        }
    }

    public async Task ActualizarAsync(int id, Cita cita)
    {
        _logger.LogInformation("Actualizando cita ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(
                "UPDATE Citas SET PacienteId = @PacienteId, MedicoId = @MedicoId, Fecha = @Fecha, Hora = @Hora WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@PacienteId", cita.PacienteId);
            cmd.Parameters.AddWithValue("@MedicoId", cita.MedicoId);
            cmd.Parameters.AddWithValue("@Fecha", cita.Fecha);
            cmd.Parameters.AddWithValue("@Hora", cita.Hora);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cita ID {Id}", id);
            throw;
        }
    }

    public async Task EliminarAsync(int id)
    {
        _logger.LogInformation("Eliminando cita ID {Id}", id);
        try
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("DELETE FROM Citas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cita ID {Id}", id);
            throw;
        }
    }
}
