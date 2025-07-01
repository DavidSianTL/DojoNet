using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AutoExpressSOAP.Data
{
    public static class dbContext
    {
        private static string _connectionString =
            ConfigurationManager.ConnectionStrings["DBAutoExpress"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static SqlCommand CreateCommand(string query, SqlConnection connection,
            CommandType commandType = CommandType.StoredProcedure)
        {
            return new SqlCommand(query, connection)
            {
                CommandType = commandType
            };
        }
    }
}