using System.Configuration;
using System.Data.SqlClient;

namespace AutoExpress.Datos.Conexion
{
    public class ConexionBD
    {
        protected SqlConnection Conexion;

        protected void Conectar()
        {
            string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
            Conexion = new SqlConnection(cadena);
            Conexion.Open();
        }

        protected void Desconectar()
        {
            if (Conexion != null && Conexion.State == System.Data.ConnectionState.Open)
                Conexion.Close();
        }
    }
}
