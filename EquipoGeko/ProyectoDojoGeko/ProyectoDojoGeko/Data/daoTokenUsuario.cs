using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Data
{
    public class daoTokenUsuario
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoTokenUsuario(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método(función) para guardar la nueva contraseña en la base de datos
        public void GuardarContrasenia(int idUsuario, string nuevaContrasenia)
        {
            // Consulta SQL para actualizar la contraseña del usuario
            string query = "UPDATE Usuarios SET Contrasenia = @nuevaContrasenia WHERE IdUsuario = @idUsuario";
            // Creamos una conexión a la base de datos usando la cadena de conexión proporcionada
            using (var connection = new SqlConnection(_connectionString))
            {
                // Abrimos la conexión a la base de datos
                connection.Open();
                // Creamos un comando SQL para ejecutar la consulta de actualización
                using (var command = new SqlCommand(query, connection))
                {
                    // Asignamos los parámetros al comando
                    command.Parameters.AddWithValue("@nuevaContrasenia", nuevaContrasenia);
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    // Ejecutamos el comando para actualizar la contraseña en la base de datos
                    command.ExecuteNonQuery();
                }
            }
        }

        // Método para validar un usuario con su nombre de usuario y contraseña
        public void GuardarToken(TokenUsuarioViewModel tokenUsuario)
        {
            // Consulta SQL para insertar un nuevo token de usuario
            string queryToken = "INSERT INTO TokenUsuario (FK_IdUsuario, Token, FechaCreacion, TiempoExpira) VALUES (@FK_IdUsuario, @Token, @FechaCreacion, @TiempoExpira)";

            // Creamos una conexión a la base de datos usando la cadena de conexión proporcionada
            using (var connection = new SqlConnection(_connectionString))
            {
                // Abrimos la conexión a la base de datos
                connection.Open();

                // Creamos un comando SQL para ejecutar la consulta de inserción
                using (var command = new SqlCommand(queryToken, connection))
                {
                    // Asignamos la conexión al comando
                    command.Connection = connection;
                    // Asignamos los parámetros al comando
                    command.Parameters.AddWithValue("@FK_IdUsuario", tokenUsuario.FK_IdUsuario);
                    command.Parameters.AddWithValue("@Token", tokenUsuario.Token);
                    command.Parameters.AddWithValue("@FechaCreacion", tokenUsuario.FechaCreacion);
                    command.Parameters.AddWithValue("@TiempoExpira", tokenUsuario.TiempoExpira);
                    // Ejecutamos el comando para insertar el token en la base de datos
                    command.ExecuteNonQuery();
                }
            }
        }

        // Validar un usuario por nombre de usuario y contraseña
        public UsuarioViewModel ValidarUsuario(string usuario, string claveIngresada)
        {
            Console.WriteLine($"=== DEBUG LOGIN ===");
            Console.WriteLine($"Usuario recibido: '{usuario}'");
            Console.WriteLine($"Clave recibida: '{claveIngresada}'");

            UsuarioViewModel user = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT IdUsuario, Username, Contrasenia, Estado, FK_IdEmpleado
                    FROM Usuarios
                    WHERE Username = @usuario AND Estado = 1", conn);

                cmd.Parameters.AddWithValue("@usuario", usuario);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Usuario encontrado en BD");
                        string hashGuardado = reader["Contrasenia"].ToString();
                        Console.WriteLine($"Hash en BD: {hashGuardado}");

                        bool esValido = BCrypt.Net.BCrypt.Verify(claveIngresada, hashGuardado);
                        Console.WriteLine($"BCrypt.Verify resultado: {esValido}");

                        if (esValido)
                        {
                            Console.WriteLine("Validación exitosa - creando usuario");
                            user = new UsuarioViewModel
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado"))
                            };
                        }
                        else
                        {
                            Console.WriteLine("BCrypt.Verify falló");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Usuario NO encontrado en BD");
                    }
                }
            }

            Console.WriteLine($"Retornando usuario: {(user != null ? "VÁLIDO" : "NULL")}");
            return user;
        }

        // Método para validar un token de usuario
        public bool ValidarToken(string token)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("sp_ValidarToken", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", token);

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
        }

        // Método para revocar un token de usuario
        public void RevocarToken(string valorToken)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("sp_RevocarToken", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", valorToken);

                cmd.ExecuteNonQuery();
            }
        }
    }
}