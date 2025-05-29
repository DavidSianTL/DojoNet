using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoPermisosWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoPermisosWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de permisos
        public async Task<List<PermisoViewModel>> ObtenerPermisosAsync()
        {
            // Declaración de la lista de permisos
            var permisos = new List<PermisoViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarPermisos";

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
                        // Mientras haya registros, los lee y los agrega a la lista de permisos
                        while (await reader.ReadAsync())
                        {
                            // Crea un nuevo objeto PermisoViewModel y lo llena con los datos del lector
                            permisos.Add(new PermisoViewModel
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("IdPermiso")),
                                NombrePermiso = reader.GetString(reader.GetOrdinal("NombrePermiso")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"))
                            });
                        }
                    }
                }
            }

            // Devuelve la lista de permisos obtenida
            return permisos;
        }

        // Método para buscar un permiso por su ID
        public async Task<PermisoViewModel> ObtenerPermisoPorIdAsync(int id)
        {
            string procedure = "sp_ListarPermisoId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agrega el parámetro del ID del permiso al comando
                    cmd.Parameters.AddWithValue("@IdPermiso", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Si hay un registro, lo lee y lo convierte a PermisoViewModel
                        if (await reader.ReadAsync())
                        {
                            return new PermisoViewModel
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("IdPermiso")),
                                NombrePermiso = reader.GetString(reader.GetOrdinal("NombrePermiso")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        // Método para insertar un nuevo permiso
        public async Task<int> InsertarPermisoAsync(PermisoViewModel permiso)
        {
            // Arreglo de parámetros requeridos por el procedimiento almacenado
            var parametros = new[]
            {
                new SqlParameter("@NombrePermiso", permiso.NombrePermiso),
                new SqlParameter("@Descripcion", permiso.Descripcion)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarPermiso", conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);

                    // Ejecuta el comando y devuelve la cantidad de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar un permiso existente
        public async Task<int> ActualizarPermisoAsync(PermisoViewModel permiso)
        {
            // Arreglo de parámetros requeridos por el procedimiento almacenado
            var parametros = new[]
            {
                new SqlParameter("@IdPermiso", permiso.IdPermiso),
                new SqlParameter("@NombrePermiso", permiso.NombrePermiso),
                new SqlParameter("@Descripcion", permiso.Descripcion)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarPermiso", conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);

                    // Ejecuta el comando y devuelve la cantidad de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar un permiso por su ID
        public async Task<int> EliminarPermisoAsync(int id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdPermiso", id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarPermiso", conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);

                    // Ejecuta el comando y devuelve la cantidad de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
