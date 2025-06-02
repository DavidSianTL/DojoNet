using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models.Usuario;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoLogWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoLogWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de logs
        public async Task<List<LogViewModel>> ObtenerLogs()
        {

            // Declaración de la lista de logs
            var logs = new List<LogViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarLogs";

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                // Abre la conexión de forma asíncrona
                await connection.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;

                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        // Mientras haya registros, los lee y los agrega a la lista de logs
                        logs.Add(new LogViewModel
                        {
                            IdLog = reader.GetInt32(reader.GetOrdinal("IdLog")),
                            FechaEntrada = reader.GetDateTime(reader.GetOrdinal("FechaEntrada")),
                            Accion = reader.GetString(reader.GetOrdinal("Accion")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
                        });

                    }
                }

                // Después de leer todos los registros, los devolvemos
                return logs;

            }
        }


        // Método para guardar un nuevo log
        public async Task<int> InsertarLogAsync(LogViewModel log)
        {

            // Validación de los parámetros del log
            var parametros = new[]
            {
                new SqlParameter("@Accion", log.Accion),
                new SqlParameter("@Descripcion", log.Descripcion),
                new SqlParameter("@Estado", log.Estado)
            };

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                // Abre la conexión de forma asíncrona
                await connection.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand("sp_InsertarLog", connection))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parametros);
                    // Ejecuta el comando y devuelve el número de filas afectadas
                    return await command.ExecuteNonQueryAsync();
                }
            }

        }



    }
}