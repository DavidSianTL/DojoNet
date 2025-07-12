using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models.Usuario;

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

        // Método(función) para generar una contraseña aleatoria
        public static string GenerarContraseniaAleatoria(int longitud = 25)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?";

            var random = new Random();
            var contrasenia = new char[longitud];

            for (int i = 0; i < longitud; i++)
            {
                contrasenia[i] = caracteres[random.Next(caracteres.Length)];
            }

            return new string(contrasenia);
        }

        // Método para obtener la lista de usuarios
        public async Task<List<UsuarioViewModel>> ObtenerUsuariosAsync()
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
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado"))
                            });
                        }
                    }
                }
            }

            // Devuelve la lista de usuarios obtenida
            return usuarios;
        }

        // Método para buscar un usuario por su ID
        public async Task<UsuarioViewModel> ObtenerUsuarioPorIdAsync(int Id)
        {
            string procedure = "sp_ListarUsuarioId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega el parámetro del ID del usuario al comando
                    cmd.Parameters.AddWithValue("@IdUsuario", Id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Si hay un registro, lo lee y lo convierte a UsuarioViewModel
                        if (await reader.ReadAsync())
                        {
                            // Crea un nuevo objeto UsuarioViewModel y lo llena con los datos del lector
                            return new UsuarioViewModel
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Contrasenia")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado"))
                            };

                        }
                    }
                }
            }

            return null;
        }

        // Método para obtener la lista de usuarios pendientes
        public async Task<List<UsuarioViewModel>> ObtenerUsuariosPendientesAsync()
        {
            var usuarios = new List<UsuarioViewModel>();
            string procedure = "sp_ListarUsuariosPendientes";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new UsuarioViewModel
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Contrasenia")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado"))
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        // Método para insertar un nuevo usuario
        public async Task<(int IdUsuario, string Contrasenia)> InsertarUsuarioAsync(UsuarioViewModel usuario)
        {
            string nuevaContrasenia = GenerarContraseniaAleatoria();
            Console.WriteLine($"[LOG] Contraseña generada: [{nuevaContrasenia}]");

            // Hasheamos la contraseña antes de guardarla
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(nuevaContrasenia);
            Console.WriteLine($"[LOG] Hash generado: [{hashPassword}]");

            // Calculamos la fecha de expiración de la contraseña (1 hora desde la fecha y hora actual)
            DateTime fechaExpiracion = DateTime.UtcNow.AddHours(1);
            usuario.FechaExpiracionContrasenia = fechaExpiracion;

            // Le pasamos el estado del usuario, el cuál tendrá que autorizarse
            int FK_IdEstado = 2; // Asignamos un estado por defecto (2 = Pendiente)

            var parametros = new[]
            {
                new SqlParameter("@Username", usuario.Username),
                new SqlParameter("@Contrasenia", hashPassword),
                new SqlParameter("@FK_IdEstado", FK_IdEstado),
                new SqlParameter("@FK_IdEmpleado", usuario.FK_IdEmpleado),
                new SqlParameter("@FechaExpiracionContrasenia", fechaExpiracion)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("sp_InsertarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);

                    // Ejecutamos y obtenemos el resultado
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int idUsuario = reader.GetInt32(0); // Obtenemos el primer campo (IdUsuario)
                            return (idUsuario, nuevaContrasenia);
                        }
                    }
                }
            }

            throw new Exception("No se pudo obtener el ID del usuario creado");
        }

        // Método para actualizar un usuario existente 
        public async Task<int> ActualizarUsuarioAsync(UsuarioViewModel usuario)
        {
            // Hasheamos la contraseña antes de guardarla
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            var parametros = new[]
            {
                    new SqlParameter("@IdUsuario", usuario.IdUsuario),
                    new SqlParameter("@Username", usuario.Username),
                    new SqlParameter("@Contrasenia", hashPassword),
                    new SqlParameter("@FK_IdEstado", usuario.FK_IdEstado),
                    new SqlParameter("@FK_IdEmpleado", usuario.FK_IdEmpleado)
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

        // Método para actualizar la contraseña y la fecha de expiración de la contraseña
        public async Task ActualizarContraseniaExpiracionAsync(int idUsuario, string hash, DateTime fechaExpiracion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarContraseniaExpiracion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Contrasenia", hash);
                    cmd.Parameters.AddWithValue("@FechaExpiracionContrasenia", fechaExpiracion);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar el estado de un usuario por su ID
        public async Task<int> ActualizarEstadoUsuarioAsync(int idUsuario, int nuevoEstado)
        {
            var parametros = new[]
            {
        new SqlParameter("@IdUsuario", idUsuario),
        new SqlParameter("@FK_IdEstado", nuevoEstado)
    };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarEstadoUsuario", conn))
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