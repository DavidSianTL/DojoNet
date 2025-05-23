using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using SistemaSeguridadMVC.Models;

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
        public async Task<List<SistemaViewModel>> ObtenerSistemasAsync()
        {

            // Crear una lista para almacenar los sistemas
            var Sistemas = new List<SistemaViewModel>();

            // Cadena de consulta SQL
            string query = "SELECT * FROM Sistemas";

            // Crear la conexión a la base de datos
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                // Abrir la conexión
                await conn.OpenAsync();

                // Crear el comando SQL
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    // Agregar parámetros para evitar inyecciones SQL
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        // Leer los resultados
                        while (await reader.ReadAsync())
                        {

                            // Crear un nuevo objeto SistemaViewModel y agregarlo a la lista
                            Sistemas.Add(new SistemaViewModel
                            {

                                // Asignar los valores leídos a las propiedades del objeto
                                IdSistema = reader.GetInt32(0),
                                NombreSistema = reader.GetString(1),
                                DescripcionSistema = reader.GetString(2)

                            });
                        }

                    }

                }

            }

            // Devolver la lista de sistemas
            return Sistemas;
        }

        // Método para ejecutar comandos INSERT
        public async Task InsertarSistemaAsync(SistemaViewModel sistema)
        {

            // Cadena de consulta SQL para insertar un nuevo sistema con parametros
            string sql = "INSERT INTO Sistemas (nombre_sistema, descripcion) VALUES (@nombre_sistema, @descripcion)";

            // Crear la conexión a la base de datos
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {

                // Abrir la conexión
                await cnn.OpenAsync();

                // Crear el comando SQL
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    // Agregar parámetros para evitar inyecciones SQL
                    cmd.Parameters.AddWithValue("@nombre_sistema", sistema.NombreSistema);
                    cmd.Parameters.AddWithValue("@descripcion", sistema.DescripcionSistema);

                    // Ejecutar el comando
                    await cmd.ExecuteNonQueryAsync();

                }


            }
        }

        // Método para obtener un sistema por ID
        public async Task<SistemaViewModel> ObtenerSistemaPorIdAsync(int Id)
        {

            // Crear un objeto SistemaViewModel para almacenar el sistema
            SistemaViewModel sistema = null;

            // Cadena de consulta SQL
            string sql = "SELECT id_sistema, nombre_sistema, descripcion FROM Sistemas WHERE id_sistema = @Id";

            // Crear la conexión a la base de datos
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {

                // Crear el comando SQL
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {

                    // Agregar parámetros para evitar inyecciones SQL
                    cmd.Parameters.AddWithValue("@Id", Id);

                    // Abrir la conexión
                    await cnn.OpenAsync();

                    // Ejecutar el comando y leer los resultados
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        // Leer el resultado
                        if (await reader.ReadAsync())
                        {

                            // Crear un nuevo objeto SistemaViewModel
                            sistema = new SistemaViewModel
                            {

                                // Asignar los valores leídos a las propiedades del objeto
                                IdSistema = reader.GetInt32(0),
                                NombreSistema = reader.GetString(1),
                                DescripcionSistema = reader.GetString(2)

                            };

                        }
                    }
                }
            }

            // Devolver el sistema encontrado
            return sistema;
        }


        // Método para actualizar un sistema
        public async Task ActualizarSistemaAsync(SistemaViewModel sistema)
        {

            // Cadena de consulta SQL para actualizar un sistema
            string sql = "UPDATE Sistemas SET nombre_sistema = @NombreSistema, descripcion = @Descripcion WHERE id_sistema = @Id";

            // Crear la conexión a la base de datos
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                // Abrir la conexión
                await conn.OpenAsync();

                // Crear el comando SQL
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    // Agregar parámetros para evitar inyecciones SQL
                    cmd.Parameters.AddWithValue("@NombreSistema", sistema.NombreSistema);
                    cmd.Parameters.AddWithValue("@Descripcion", sistema.DescripcionSistema);
                    cmd.Parameters.AddWithValue("@Id", sistema.IdSistema);

                    // Ejecutar el comando
                    await cmd.ExecuteNonQueryAsync();


                }
            }

        }


    }
}
