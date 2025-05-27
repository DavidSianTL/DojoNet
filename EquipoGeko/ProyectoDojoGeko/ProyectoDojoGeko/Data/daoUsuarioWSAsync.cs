using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoUsuarioWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoUsuarioWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de empleados (usuarios)
        public async Task<List<UsuarioViewModel>> ObtenerEmpleadosAsync()
        {

            // Declaración de la lista de usuarios
            var usuarios = new List<UsuarioViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarUsuarios";

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

                        // Mientras haya registros, los lee y los agrega a la lista de usuarios
                        while (await reader.ReadAsync())
                        {

                            // Crea un nuevo objeto UsuarioViewModel y lo llena con los datos del lector
                            usuarios.Add(new UsuarioViewModel
                            {

                                // Asigna los valores de las columnas del lector a las propiedades del modelo
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Contrasenia")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
                            });
                        }
                    }
                }
            }

            // Devuelve la lista de usuarios obtenida
            return usuarios;
        }

        // Método para buscar un usuario por su ID
        public async Task<int> ObtenerUsuarioPorIdAsync(int Id)
        {
            var parametros = new[]
            {
                    new SqlParameter("@IdUsuario", Id)
                };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ListarUsuarioId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return (int)await cmd.ExecuteScalarAsync();
                }
            }
        }

        // Método para insertar un nuevo usuario
        public async Task<int> InsertarUsuarioAsync(UsuarioViewModel usuario)
        {
            var parametros = new[]
            {
                    new SqlParameter("@Nombre", usuario.Username),
                    new SqlParameter("@Password", usuario.Password),
                    new SqlParameter("@FechaCreacion", usuario.FechaCreacion)
                };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar un usuario existente 
        public async Task<int> ActualizarUsuarioAsync(UsuarioViewModel usuario)
        {
            var parametros = new[]
            {
                    new SqlParameter("@IdUsuario", usuario.IdUsuario),
                    new SqlParameter("@Nombre", usuario.Username),
                    new SqlParameter("@Password", usuario.Password),
                    new SqlParameter("@FechaCreacion", usuario.FechaCreacion),
                    new SqlParameter("@Estado", usuario.Estado)
                };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar (cambiar su estado) un usuario por su ID
        public async Task<int> EliminarUsuarioAsync(int Id)
        {
            var parametros = new[]
            {
                    new SqlParameter("@IdUsuario", Id)
                };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}