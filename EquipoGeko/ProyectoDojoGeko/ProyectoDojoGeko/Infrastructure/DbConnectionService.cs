using System.Data;
using Microsoft.Data.SqlClient;

namespace ProyectoDojoGeko.Infrastructure
{
    public class DbConnectionService
    {
        private readonly string _connectionString;

        public DbConnectionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> ExecuteNonQueryAsync(string spName, Dictionary<string, object> parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(spName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (var param in parameters)
                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }
    }
}
