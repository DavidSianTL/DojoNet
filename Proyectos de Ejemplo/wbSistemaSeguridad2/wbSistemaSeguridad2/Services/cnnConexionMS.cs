
using Microsoft.Data.SqlClient;
using System;
using System.Data;


namespace wbSistemaSeguridad2.Services
{
    public class cnnConexionMS
    {


        private string _connectionString;

        public cnnConexionMS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataSet EjecutarSelect(string query)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar SELECT: " + ex.Message);
            }
            return ds;
        }

        // Ejecutar comandos como INSERT, UPDATE o DELETE
        public int EjecutarComando(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0 ? 1 : -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar comando SQL: " + ex.Message);
                return -1;
            }
        }

        // Ejecutar procedimientos almacenados con parámetros
        public int EjecutarProcedimiento(string nombreProcedimiento, SqlParameter[] parametros)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar procedimiento almacenado: " + ex.Message);
                return -1;
            }
        }

    }
}
