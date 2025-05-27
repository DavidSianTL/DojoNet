using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace MVCdaoWS.Services
{
    public class cnnConexionAsync
    {
        private readonly string _connectionString;

        public cnnConexionAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para ejecutar un SELECT y retornar un DataSet
        public async Task<DataSet> EjecutarSelectAsync(string query)
        {
            using (var conexion = new SqlConnection(_connectionString))
            {
                await conexion.OpenAsync();

                using (var comando = new SqlCommand(query, conexion))
                {
                    using (var adaptador = new SqlDataAdapter(comando))
                    {
                        var ds = new DataSet();
                        adaptador.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        // Método para ejecutar un procedimiento almacenado (INSERT, UPDATE, DELETE)
        public async Task<int> EjecutarProcedimientoAsync(string nombreProcedimiento, SqlParameter[] parametros)
        {
            using (var conexion = new SqlConnection(_connectionString))
            {
                await conexion.OpenAsync();

                using (var comando = new SqlCommand(nombreProcedimiento, conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    if (parametros != null)
                    {
                        comando.Parameters.AddRange(parametros);
                    }

                    return await comando.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
