using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace SistemaSeguridadMVC.Services
{
    public class cnnConexionAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public cnnConexionAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Conexión a la base de datos
        public async Task<DataSet> EjecutarSelectAsync(string query)
        {
            // Crear un DataSet para almacenar los resultados
            DataSet _dataSet = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    
                    adapter.Fill(_dataSet); // SqlDataAdapter no tiene FillAsync, así que sigue siendo síncrono
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al ejecutar SELECT: " + e.Message);
            }

            // Asegurarse de que siempre se devuelve un valor
            return _dataSet;
        }

        // Método para ejecutar comandos INSERT, UPDATE y DELETE
        public async Task<int> EjecutarComandoAsync(string query)
        {
            try
            {
                
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Esperar a que la conexión se abra
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ejecutar el comando asincrónicamente
                        int filasAfectadas = await command.ExecuteNonQueryAsync();
                        // Devolver el número de filas afectadas
                        return filasAfectadas > 0 ? 1 : -1;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error al ejecutar comando: " + e.Message);
            }

            return 0;

        }

        // Método para ejectuar un comando para los SP
        public async Task<int> EjecutarProcedimientoAsync(string nombreProcedimiento, SqlParameter[] parametros)
        {
            try
            {
                // Usamos using para asegurar que la conexión se cierre correctamente
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Esperar a que la conexión se abra
                    await connection.OpenAsync();

                    // Crear un SqlCommand para ejecutar el procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                    {

                        // Tipo de comando para procedimientos almacenados
                        command.CommandType = CommandType.StoredProcedure;
                        if(parametros != null)
                        {
                            // Agregar los parámetros al comando
                            command.Parameters.AddRange(parametros);
                        }

                        // Esperamos que el comando se ejecute
                        await command.ExecuteNonQueryAsync();
                        return 1;
                    }

                }

            }
            catch(Exception e)
            {
                Console.WriteLine("Error al ejecutar procedimiento: " + e.Message);
                return -1;
            }

        }




    }
}
