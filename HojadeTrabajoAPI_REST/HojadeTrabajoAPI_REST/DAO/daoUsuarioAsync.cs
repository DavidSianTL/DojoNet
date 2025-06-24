using Microsoft.Data.SqlClient;
using HojadeTrabajoAPI_REST.DATA;
using HojadeTrabajoAPI_REST.Models;
using HojadeTrabajoAPI_REST.Exceptions;

namespace HojadeTrabajoAPI_REST.DAO
{
    public class daoUsuarioAsync
    {
        private readonly DbConnection _db;

        public daoUsuarioAsync(DbConnection db)
        {
            _db = db;

        }

        public async Task<Usuario> ObtenerUsuarioAsync(string usuarioA)
        {
            Usuario usuario = null;

            string query = @"SELECT Id
                            , UsuarioS
                            , Password
                                FROM Usuario 
                            WHERE UsuarioS = @UsuarioS";
            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioS", usuarioA);

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = (int)reader["Id"],
                        UsuarioS = reader["UsuarioS"].ToString(),
                        Password = reader["Password"].ToString(), // NECESARIO para login
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: al obtener el usuario " + ex.Message, ex);
            }

            return usuario;
        }
        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var ListaU = new List<Usuario>();
            string query = "";

            query = "SELECT Id, UsuarioS FROM Usuario";

            using var cnn = _db.GetConnection();
            try
            {
                await cnn.OpenAsync();
                var cmd = new SqlCommand(query, cnn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ListaU.Add(new Usuario
                    {
                        IdUsuario = (int)reader["Id"],
                        UsuarioS = reader["UsuarioS"].ToString(),
                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL OBTENER LOS USUARIOS." + ex.Message, ex);

            }
            return ListaU;

        }

        public async Task InsertarUsuarioAsync(Usuario usuario)
        {
            string query = "";
            var contraseniaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            query = "INSERT INTO Usuario (UsuarioS, Password) VALUES (@UsuarioS, @Password)";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioS", usuario.UsuarioS);
                cmd.Parameters.AddWithValue("@Password", contraseniaHash);

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL INSERTAR USUARIOS." + ex.Message, ex);

            }

        }
        private async Task<bool> ValidarContraseniaCambioAsync(string ContraseniaCambio, int IdUsuario)
        {
            bool contraseniaCambio = false;
            string contraseniaHashAlmacenada = "";
            int filasConsultadas = 0;
            string query = "SELECT Password FROM Usuario WHERE Id = @IdUsuario";

            try
            {
                using (var conn = _db.GetConnection())
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {

                        contraseniaHashAlmacenada = reader["Password"].ToString();
                    }
                }

                if (contraseniaHashAlmacenada == "")
                {
                    throw new NotFoundException($"Usuario con id {IdUsuario} no encontrado.");
                }

                contraseniaCambio = !BCrypt.Net.BCrypt.Verify(ContraseniaCambio, contraseniaHashAlmacenada);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL VALIDAR CAMBIO DE CONTRASEÑA." + ex.Message, ex);

            }
            return contraseniaCambio;
        }


        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            bool CambioContrasenia = false;
            string query = "";
            int filasAfectadas = 0;

            query = "UPDATE Usuario SET UsuarioS = @UsuarioS";

            if (!string.IsNullOrWhiteSpace(usuario.Password))
            {
                CambioContrasenia = await ValidarContraseniaCambioAsync(usuario.Password, usuario.IdUsuario);
            }

            if (CambioContrasenia)
            {
                query += ",Password = @PasswordA";

            }
            query += " WHERE Id = @IdUsuario";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@UsuarioS", usuario.UsuarioS);

                if (CambioContrasenia)
                {
                    var nuevaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                    cmd.Parameters.AddWithValue("@PasswordA", nuevaHash);
                }
                filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"Usuario con id {usuario.IdUsuario} no encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ACTUALIZAR EL USUARIO." + ex.Message, ex);

            }
        }


        public async Task EliminarUsuarioAsync(int id)
        {

            string query = "";
            int filasAfectadas = 0;

            query = "DELETE FROM Usuario WHERE Id = @IdUsuario";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"Usuario con id {id} no encontrado.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ELIMINAR EL USUARIO." + ex.Message, ex);

            }

        }
    }
}
