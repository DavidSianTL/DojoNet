using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClinicaApi.DAL;

public class UsuarioDao
{
    private readonly string _connectionString;

    public UsuarioDao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb")!;
    }

    // ✅ Método usado en el login actual (con BCrypt)
    public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
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
                // Si tenés Rol en tu tabla: Rol = reader["Rol"].ToString()!
            };
        }

        return null;
    }

    // ❌ OPCIONAL: Método viejo que comparaba contraseñas sin hash (no recomendado)
    public async Task<Usuario?> ValidarCredencialesAsync(string username, string password)
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

        return null;
    }
}
