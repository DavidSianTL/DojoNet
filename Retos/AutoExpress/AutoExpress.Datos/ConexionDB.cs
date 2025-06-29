using System.Configuration;
using System.Data.SqlClient;

namespace AutoExpress.Datos
{
    public class ConexionDB
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["AutoExpressDB"].ConnectionString;

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }

        public static string ObtenerCadenaConexion()
        {
            return connectionString;
        }
    }
}
