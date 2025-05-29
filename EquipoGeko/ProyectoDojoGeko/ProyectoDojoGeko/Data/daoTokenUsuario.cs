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

        // Método para validar un usuario con su nombre de usuario y contraseña
        

        public void GuardarToken(TokenUsuarioViewModel tokenUsuario)
        {
            // Consulta SQL para insertar un nuevo token de usuario
            string queryToken = "INSERT INTO UsuarioToken (FK_IdUsuario, Token, FechaCreacion, TiempoExpira) VALUES (@FK_IdUsuario, @Token, @FechaCreacion, @TiempoExpira)";

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
            UsuarioViewModel user = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    SELECT id_usuario, usuario, nom_usuario, contrasenia, fk_id_estado
                    FROM Usuarios
                    WHERE usuario = @usuario AND fk_id_estado = 1", conn);

                cmd.Parameters.AddWithValue("@usuario", usuario);


                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashGuardado = reader["contrasenia"].ToString();

                        if (BCrypt.Net.BCrypt.Verify(claveIngresada, hashGuardado))
                        {
                            user = new UsuarioViewModel
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Contrasenia")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado"))

                            };
                        }
                    }
                }
            }

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
