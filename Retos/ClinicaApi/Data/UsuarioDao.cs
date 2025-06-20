using ClinicaApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClinicaApi.DAL;

public class UsuarioDao
{
    private readonly string _connectionString;

    public UsuarioDao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ClinicaDb");
    }

    public async Task<Usuario?> ValidarCredencialesAsync(string username, string password)
    {
        using SqlConnection conn = new(_connectionString);
        using SqlCommand cmd = new("SELECT * FROM Usuarios WHERE Username = @Username AND Password = @Password", conn);
        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Password", password);
        await conn.OpenAsync();
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
