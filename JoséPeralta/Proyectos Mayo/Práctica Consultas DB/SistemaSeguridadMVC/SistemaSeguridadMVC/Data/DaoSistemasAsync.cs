using Microsoft.Data.SqlClient;
using SistemaSeguridadMVC.Models;

namespace SistemaSeguridadMVC.Data
{
    public class DaoSistemasAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public DaoSistemasAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener todos los sistemas
        public async Task<List<SistemaViewModel>> ObtenerSistemasAsync()
        {

            // Creamos una lista para almacenar los sistemas
            var Sistemas = new List<SistemaViewModel>();

            // Creamos el quuery para obtener todos los sistemas
            string query = "SELECT * FROM Sistemas";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abrimos nuestra conexión
                await connection.OpenAsync();

                // Creamos un SqlCommand para ejecutar el query
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // Ejecutamos el comando asincrónicamente
                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        // Leemos los datos de cada fila
                        while (await reader.ReadAsync())
                        {

                            // Leemos los datos de cada fila
                            // y los agregamos a la lista de sistemas
                            // usando el modelo SistemaViewModel
                            Sistemas.Add(new SistemaViewModel()
                            {
                                IdSistema = reader.GetInt32(0),
                                NombreSistema = reader.GetString(1),
                                DescripcionSistema = reader.GetString(2)
                            });

                        }

                    }


                }

            }

            // Devolvemos la lista de sistemas
            return Sistemas;

        }

        // Método para insertar un nuevo sistema
        public async Task<int> InsertarSistemaAsync(SistemaViewModel sistema)
        {
            // Creamos el query para insertar un nuevo sistema
            string query = $"INSERT INTO Sistemas (NombreSistema, DescripcionSistema) VALUES ('{sistema.NombreSistema}', '{sistema.DescripcionSistema}')";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abrimos nuestra conexión
                await connection.OpenAsync();
                // Creamos un SqlCommand para ejecutar el query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ejecutamos el comando asincrónicamente
                    int filasAfectadas = await command.ExecuteNonQueryAsync();
                    return filasAfectadas > 0 ? 1 : -1;
                }
            }
        }

        // Método para actualizar un sistema existente
        


    }
}
