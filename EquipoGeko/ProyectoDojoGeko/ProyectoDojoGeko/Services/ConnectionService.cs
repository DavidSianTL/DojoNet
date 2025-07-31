using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Services
{
    public interface IConnectionService
    {
        Task<DataSet> EjecutarSelectAsync(string query);
        Task<int> EjecutarComandoAsync(string sql);
        Task<int> EjecutarProcedimientoNonQueryAsync(string procedimiento, SqlParameter[]? parametros = null);
        Task<DataSet> EjecutarProcedimientoAsync(string procedimiento, SqlParameter[]? parametros = null);
    }

    public class ConnectionService : IConnectionService
    {

        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la cadena de conexión
        public ConnectionService(string connectionString)
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

        // Método para ejecutar procedimientos almacenados sin retorno de datos
        public async Task<int> EjecutarProcedimientoNonQueryAsync(string procedimiento, SqlParameter[]? parametros = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(procedimiento, conn))
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


        // Método para ejecutar procedimientos almacenados que devuelven datos
        public async Task<DataSet> EjecutarProcedimientoAsync(string procedimiento, SqlParameter[]? parametros = null)
        {
            DataSet ds = new DataSet();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                using var cmd = new SqlCommand(procedimiento, conn);
                    
                cmd.CommandType = CommandType.StoredProcedure;

                if (parametros != null) cmd.Parameters.AddRange(parametros);

                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds); // SqlDataAdapter no tiene FillAsync

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar procedimiento almacenado: " + ex.Message);
            }
                
            return ds;

        }

    }
}