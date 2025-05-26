using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;


namespace wbSistemaSeguridadMVC.Services
{
    public class cnnConexionAsync
    {
        private readonly string _connectionString;

        public cnnConexionAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DataSet> EjecutarSelectAsync(string query)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                   
                    adapter.Fill(ds); // SqlDataAdapter no tiene FillAsync, así que sigue siendo síncrono
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar SELECT: " + ex.Message);
            }
            return ds;
        }


        public async Task<int> EjecutarComandoAsync(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                        return filasAfectadas > 0 ? 1 : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar comando SQL: " + ex.Message);
                return -1;
            }
        }

        public async Task<int> EjecutarProcedimientoAsync(string nombreProcedimiento, SqlParameter[] parametros)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                        }

                        await cmd.ExecuteNonQueryAsync();
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
