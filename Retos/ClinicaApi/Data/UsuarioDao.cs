using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaApi.DAL;

public class UsuarioDao
{
    private readonly string _connectionString;
    private readonly ILogger<UsuarioDao> _logger;

    public UsuarioDao(IConfiguration configuration, ILogger<UsuarioDao> logger)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb")!;
        _logger = logger;
    }

    // ✅ Método usado en el login actual (con BCrypt)
    public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
    {
        _logger.LogInformation("Buscando usuario por username: {Username}", username);

        try
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT * FROM Usuarios WHERE Username = @Username";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)reader["Id"],
                    Username = reader["Username"].ToString()!,
                    Password = reader["Password"].ToString()!
                    // Rol = reader["Rol"].ToString()! ← si usás roles
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por username: {Username}", username);
        }

        return null;
    }

    // ❌ OPCIONAL: Método viejo que comparaba contraseñas sin hash (no recomendado)
    public async Task<Usuario?> ValidarCredencialesAsync(string username, string password)
    {
        _logger.LogWarning("Usando método obsoleto para validar credenciales");

        try
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT * FROM Usuarios WHERE Username = @Username AND Password = @Password";
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)reader["Id"],
                    Username = reader["Username"].ToString()!,
                    Password = reader["Password"].ToString()!
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en validación de credenciales (obsoleta) para usuario: {Username}", username);
        }

        return null;
    }
}
