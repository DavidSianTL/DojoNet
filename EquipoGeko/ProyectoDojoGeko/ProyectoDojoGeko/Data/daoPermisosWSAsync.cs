using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoPermisosWSAsync
    {
        
        private readonly string _connectionString; // Cadena de conexión a la base de datos
        /// Constructor que recibe la cadena de conexión
        public daoPermisosWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// Método para obtener la lista de permisos
        public async Task<List<PermisoViewModel>> ObtenerPermisosAsync()
        {
            // Utiliza el procedimiento almacenado para obtener la lista de permisos
            var permisos = new List<PermisoViewModel>();
            //Utiliza el procedimiento almacenado para obtener la lista de permisos
            string procedure = "sp_ListarPermisos"; 
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega los parámetros necesarios si el procedimiento almacenado los requiere
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Lee los resultados del procedimiento almacenado
                        while (await reader.ReadAsync())
                        {
                            permisos.Add(new PermisoViewModel
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("IdPermiso")),
                                NombrePermiso = reader.GetString(reader.GetOrdinal("NombrePermiso")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) // Nuevo campo
                            });
                        }
                    }
                }
            }
            // Si no se encuentran permisos, retornar una lista vacía
            return permisos;
        }
        /// Método para obtener un permiso por su ID
        public async Task<PermisoViewModel> ObtenerPermisoPorIdAsync(int id)
        {
            // Utiliza el procedimiento almacenado para obtener un permiso por su ID
            string procedure = "sp_ListarPermisoId";
            // Crea una conexión a la base de datos y ejecuta el procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión a la base de datos
                await conn.OpenAsync();
                // Utiliza el procedimiento almacenado para obtener un permiso por su ID
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega el parámetro necesario para el procedimiento almacenado
                    cmd.Parameters.AddWithValue("@IdPermiso", id);
                    // Ejecuta el comando y obtiene un lector de datos
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Lee los resultados del procedimiento almacenado
                        if (await reader.ReadAsync())
                        {
                            return new PermisoViewModel
                            {
                                IdPermiso = reader.GetInt32(reader.GetOrdinal("IdPermiso")),
                                NombrePermiso = reader.GetString(reader.GetOrdinal("NombrePermiso")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")) 
                            };
                        }
                    }
                }
            }
            // Si no se encuentra el permiso, retornar null
            return null;
        }
        /// Método para insertar un nuevo permiso
        public async Task<int> InsertarPermisoAsync(PermisoViewModel permiso)
        {
            // Verifica que el permiso no sea nulo
            var parametros = new[]
            {
                new SqlParameter("@NombrePermiso", permiso.NombrePermiso),
                new SqlParameter("@Descripcion", permiso.Descripcion),
                new SqlParameter("@Estado", permiso.Estado)
            };
            // Crea una conexión a la base de datos y ejecuta el procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                // Utiliza el procedimiento almacenado para insertar un nuevo permiso
                using (SqlCommand cmd = new SqlCommand("sp_InsertarPermiso", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar un permiso existente
        public async Task<int> ActualizarPermisoAsync(PermisoViewModel permiso)
        {
            // Verifica que el permiso no sea nulo y tenga un IdPermiso válido
            var parametros = new[]
            {
                new SqlParameter("@IdPermiso", permiso.IdPermiso),
                new SqlParameter("@NombrePermiso", permiso.NombrePermiso),
                new SqlParameter("@Descripcion", permiso.Descripcion),
                new SqlParameter("@Estado", permiso.Estado)
            };
            // Crea una conexión a la base de datos y ejecuta el procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                // Utiliza el procedimiento almacenado para actualizar un permiso existente
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarPermiso", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar un permiso (cambiar su estado a false)
        public async Task<int> EliminarPermisoAsync(int id) 
        {
            // Verifica que el id del permiso sea válido
            var parametros = new[]
            {
                new SqlParameter("@IdPermiso", id)
            };
            // Crea una conexión a la base de datos y ejecuta el procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                // Utiliza el procedimiento almacenado para eliminar un permiso
                using (SqlCommand cmd = new SqlCommand("sp_EliminarPermiso", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        } 
    }
}
