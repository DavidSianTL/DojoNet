using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Services
{
    public class cnnConexionWSAsync
    {

        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la cadena de conexión
        public cnnConexionWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Para ejecutar consultas SELECT y devolver un DataSet
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

        // Para ejecutar comandos SQL que no devuelven resultados (INSERT, UPDATE, DELETE)
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

        // Para ejecutar procedimientos almacenados
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