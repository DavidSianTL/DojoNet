using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoBitacoraWSAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoBitacoraWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de bitácoras
        public async Task<List<BitacoraViewModel>> ObtenerBitacorasAsync()
        {
            // Declaración de la lista de bitácoras
            var bitacoras = new List<BitacoraViewModel>();
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarBitacoras";
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
                        // Mientras haya registros, los lee y los agrega a la lista de bitácoras
                        while (await reader.ReadAsync())
                        {
                            bitacoras.Add(new BitacoraViewModel
                            {
                                IdBitacora = reader.GetInt32(reader.GetOrdinal("IdBitacora")),
                                FechaEntrada = reader.GetDateTime(reader.GetOrdinal("FechaEntrada")),
                                Accion = reader.GetString(reader.GetOrdinal("Accion")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdUsuario = reader.GetInt32(reader.GetOrdinal("FK_IdUsuario")),
                                FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                            });
                        }
                    }
                }
            }
            // Después de leer todos los registros, los devolvemos
            return bitacoras;
        }

        // Método para guardar una nueva bitácora
        public async Task<int> InsertarBitacoraAsync(BitacoraViewModel bitacora)
        {

            // Validación de los parámetros de entrada
            var parametros = new[]
            {
                new SqlParameter("@Accion", SqlDbType.NVarChar, 75) { Value = bitacora.Accion },
                new SqlParameter("@Descripcion", SqlDbType.NVarChar, 255) { Value = bitacora.Descripcion },
                new SqlParameter("@FK_IdUsuario", SqlDbType.Int) { Value = bitacora.FK_IdUsuario },
                new SqlParameter("@FK_IdSistema", SqlDbType.Int) { Value = bitacora.FK_IdSistema }
            };

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await connection.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand("sp_InsertarBitacora", connection))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parametros);
                    // Ejecuta el comando y obtiene el ID de la nueva bitácora
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }

            }

        }


    }
}
