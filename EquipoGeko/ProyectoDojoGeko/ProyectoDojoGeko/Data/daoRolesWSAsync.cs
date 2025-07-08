using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoRolesWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoRolesWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de roles
        public async Task<List<RolesViewModel>> ObtenerRolesAsync()
        {
            // Declaración de la lista de roles
            var roles = new List<RolesViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarRoles";

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
                        // Mientras haya registros, los lee y los agrega a la lista de roles
                        while (await reader.ReadAsync())
                        {
                            // Crea un nuevo objeto RolesViewModel y lo llena con los datos del lector
                            roles.Add(new RolesViewModel
                            {
                                // Asigna los valores de las columnas del lector a las propiedades del modelo
                                IdRol = reader.GetInt32(reader.GetOrdinal("IdRol")),
                                NombreRol = reader.GetString(reader.GetOrdinal("NombreRol")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }
            // Devuelve la lista de roles obtenida
            return roles;
        }

        // Método para buscar rol por su ID
        public async Task<RolesViewModel> ObtenerRolPorIdAsync(int id)
        {
            // Nombre del procedimiento a ejecutar
            string procedure = "sp_ListarRolId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parámetro del ID del rol
                    cmd.Parameters.AddWithValue("@IdRol", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Si se encuentra un rol con ese ID, lo devuelve
                        if (await reader.ReadAsync())
                        {
                            return new RolesViewModel
                            {
                                IdRol = reader.GetInt32(reader.GetOrdinal("IdRol")),
                                NombreRol = reader.GetString(reader.GetOrdinal("NombreRol")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            };
                        }
                    }
                }
            }
            // Si no se encontró ningún registro, devuelve null
            return null;
        }

        // Método para agregar un nuevo rol
        public async Task<int> InsertarRolAsync(RolesViewModel rol)
        {
            // Arreglo de parámetros requeridos por el procedimiento almacenado
            var parametros = new[]
            {
                new SqlParameter("@NombreRol", rol.NombreRol),
                new SqlParameter("@FK_IdEstado", rol.FK_IdEstado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarRol", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    // Ejecuta la inserción y devuelve el número de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar un rol existente
        public async Task<int> ActualizarRolAsync(RolesViewModel rol)
        {
            // Parámetros requeridos para actualizar el rol
            var parametros = new[]
            {
                new SqlParameter("@IdRol", rol.IdRol),
                new SqlParameter("@NombreRol", rol.NombreRol),
                new SqlParameter("@FK_IdEstado", rol.FK_IdEstado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarRol", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    // Ejecuta la actualización y devuelve las filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar un rol por su ID
        public async Task<int> EliminarRolAsync(int id)
        {
            // Parámetro con el ID del rol a eliminar
            var parametros = new[]
            {
                new SqlParameter("@IdRol", id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarRol", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    // Ejecuta el borrado y devuelve las filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        // Método para desactivar un rol por su ID
        public async Task DesactivarRolAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("UPDATE Roles SET Estado = 0 WHERE IdRol = @IdRol", conn))
            {
                cmd.Parameters.AddWithValue("@IdRol", id);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}
