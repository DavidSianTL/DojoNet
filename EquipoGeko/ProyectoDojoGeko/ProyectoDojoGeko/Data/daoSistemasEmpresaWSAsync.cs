using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models.SistemasEmpresa;

namespace ProyectoDojoGeko.Data
{
    public class daoSistemasEmpresaWSAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoSistemasEmpresaWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de Empresas y sus Sistemas
        public async Task<List<SistemasEmpresaViewModel>> ObtenerSistemasEmpresaAsync()
        {
            // Declaración de la lista de sistemas de empresa
            var sistemasEmpresa = new List<SistemasEmpresaViewModel>();
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarSistemasEmpresa";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Recorre los resultados del lector
                        while (await reader.ReadAsync())
                        {
                            // Crea una nueva instancia del modelo SistemasEmpresaViewModel
                            var sistemaEmpresa = new SistemasEmpresaViewModel
                            {
                                IdSistemasEmpresa = reader.GetInt32(0),
                                FK_IdEmpresa = reader.GetInt32(1),
                                FK_IdSistema = reader.GetInt32(2)
                            };
                            // Agrega el sistema a la lista
                            sistemasEmpresa.Add(sistemaEmpresa);
                        }
                    }
                }
            }
            return sistemasEmpresa;
        }

        // Método para buscar una relación de sistema y empresa por su ID
        public async Task<SistemasEmpresaViewModel?> BuscarSistemasEmpresaPorIdAsync(int idSistemasEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_BuscarSistemasEmpresaPorId";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega el parámetro de entrada para el ID del sistema de empresa
                    cmd.Parameters.AddWithValue("@IdSistemasEmpresa", idSistemasEmpresa);
                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Verifica si hay resultados
                        if (await reader.ReadAsync())
                        {
                            // Crea una nueva instancia del modelo SistemasEmpresaViewModel
                            var sistemaEmpresa = new SistemasEmpresaViewModel
                            {
                                IdSistemasEmpresa = reader.GetInt32(0),
                                FK_IdEmpresa = reader.GetInt32(1),
                                FK_IdSistema = reader.GetInt32(2)
                            };
                            return sistemaEmpresa;
                        }
                    }
                }
            }
            return null; // Retorna null si no se encuentra el sistema de empresa

        }

        // Método para insertar una nueva relación de sistema y empresa
        public async Task<bool> InsertarSistemasEmpresaAsync(SistemasEmpresaViewModel sistemaEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_InsertarSistemasEmpresa";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega los parámetros de entrada para el sistema de empresa
                    cmd.Parameters.AddWithValue("@FK_IdEmpresa", sistemaEmpresa.FK_IdEmpresa);
                    cmd.Parameters.AddWithValue("@FK_IdSistema", sistemaEmpresa.FK_IdSistema);
                    // Ejecuta el comando y obtiene el número de filas afectadas
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // Retorna true si se insertó correctamente
                }
            }
        }

        // Método para actualizar una relación de sistema y empresa
        public async Task<bool> ActualizarSistemasEmpresaAsync(SistemasEmpresaViewModel sistemaEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ActualizarSistemasEmpresa";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega los parámetros de entrada para el sistema de empresa
                    cmd.Parameters.AddWithValue("@IdSistemasEmpresa", sistemaEmpresa.IdSistemasEmpresa);
                    cmd.Parameters.AddWithValue("@FK_IdEmpresa", sistemaEmpresa.FK_IdEmpresa);
                    cmd.Parameters.AddWithValue("@FK_IdSistema", sistemaEmpresa.FK_IdSistema);
                    // Ejecuta el comando y obtiene el número de filas afectadas
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // Retorna true si se actualizó correctamente
                }
            }
        }

        // Método para eliminar una relación de sistema y empresa por su ID
        public async Task<bool> EliminarSistemasEmpresaAsync(int idSistemasEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_EliminarSistemasEmpresa";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega el parámetro de entrada para el ID del sistema de empresa
                    cmd.Parameters.AddWithValue("@IdSistemasEmpresa", idSistemasEmpresa);
                    // Ejecuta el comando y obtiene el número de filas afectadas
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // Retorna true si se eliminó correctamente
                }
            }
        }   

         // Método para obtener la vista combinada de empresas y sistemas
        public async Task<List<VistaSistemasEmpresaViewModel>> ObtenerVistaSistemasEmpresaAsync()
        {
            var lista = new List<VistaSistemasEmpresaViewModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT * FROM SistemasEmpresaView", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new VistaSistemasEmpresaViewModel
                        {
                            IdSistemasEmpresa = reader.GetInt32(reader.GetOrdinal("IdSistemasEmpresa")),
                            IdEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpresa")),
                            NombreEmpresa = reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                            IdSistema = reader.GetInt32(reader.GetOrdinal("IdSistema")),
                            NombreSistema = reader.GetString(reader.GetOrdinal("NombreSistema"))
                        });
                    }
                }
            }
            return lista;
        }
    }
}
