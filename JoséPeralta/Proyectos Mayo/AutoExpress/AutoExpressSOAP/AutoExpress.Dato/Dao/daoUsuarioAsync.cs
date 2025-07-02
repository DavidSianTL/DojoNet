using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AutoExpressSOAP.Data;
using AutoExpressSOAP.Models;

namespace AutoExpressSOAP.Dao
{
    public class daoUsuarioAsync
    {

        // Obtiene un usuario por su ID
        public async Task<UsuariosViewModel> ObtenerUsuarioPorIdAsync(int id)
        {

            // Creamos la conexión
            using (var connection = dbContext.GetConnection())
            {

                // Pasamos el procedimiento almacenado
                var command = dbContext.CreateCommand("sp_ObtenerUsuarioPorId", connection);

                // Pasamos el parámetro del ID
                command.Parameters.AddWithValue("@IdUsuario", id);

                // Abrimos la conexión
                await connection.OpenAsync();

                // Ejecutamos el comando
                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    // Verificamos si hay datos
                    if (await reader.ReadAsync())
                    {
                        // Retornamos el usuario
                        return new UsuariosViewModel
                        {
                            // Datos del usuario
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("idUsuario")),
                            NombreCompleto = reader.GetString(reader.GetOrdinal("nombreCompleto")),
                            Usuario = reader.GetString(reader.GetOrdinal("usuario")),
                            Contrasenia = reader.GetString(reader.GetOrdinal("contrasenia")),
                            Token = reader.GetString(reader.GetOrdinal("token"))
                        };
                    }
                }
                return null;
            }
        }

        // Autenticar usuario
        public async Task<UsuariosViewModel> AutenticarUsuarioAsync(string usuario, string contrasenia)
        {

            // Creamos la conexión
            using (var connection = dbContext.GetConnection())
            {

                // Pasamos el procedimiento almacenado
                var command = dbContext.CreateCommand("sp_AutenticarUsuario", connection);

                // Pasamos el parámetro del ID
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contrasenia", contrasenia);

                // Abrimos la conexión
                await connection.OpenAsync();

                // Ejecutamos el comando
                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        return new UsuariosViewModel
                        {
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("idUsuario")),
                            NombreCompleto = reader.GetString(reader.GetOrdinal("nombreCompleto")),
                            Usuario = reader.GetString(reader.GetOrdinal("usuario")),
                            Contrasenia = reader.GetString(reader.GetOrdinal("contrasenia")),
                            Token = reader.GetString(reader.GetOrdinal("token"))
                        };
                    }
                }
                return null;
            }
        }

    }
}