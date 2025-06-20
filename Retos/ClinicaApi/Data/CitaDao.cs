using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClinicaApi.DAL;

public class CitaDao
{
    private readonly string _connectionString;

    public CitaDao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
    }

    public async Task<List<Cita>> ObtenerTodasAsync()
    {
        var lista = new List<Cita>();
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
                Hora = (TimeSpan)reader["Hora"]
            });
        }

        return lista;
    }

    public async Task<Cita?> ObtenerPorIdAsync(int id)
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
                Hora = (TimeSpan)reader["Hora"]
            };
        }

        return null;
    }

    public async Task CrearAsync(Cita cita)
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

    public async Task ActualizarAsync(int id, Cita cita)
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

    public async Task EliminarAsync(int id)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("DELETE FROM Citas WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
