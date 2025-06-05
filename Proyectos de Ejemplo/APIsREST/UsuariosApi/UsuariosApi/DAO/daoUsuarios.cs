using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using UsuariosApi.Models;
using UsuariosApi.Data;



namespace UsuariosApi.DAO
{
    public class daoUsuarios
    {
        private readonly DbConnection _db;
        public const int ESTADO_ELIMINADO = 4;

        public daoUsuarios(DbConnection db)
        {
            _db = db;

        }
        public List<Usuario> ObtenerUsuarios() 
        {
            var ListaU = new List<Usuario>();
            string query = "";

            query = $@"SELECT u.id_usuario as id_usuario,
                        u.usuario  as usuario, 
                        u.nom_usuario as nom_usuario, 
                        u.fk_id_estado as fk_id_estado, 
                        u.fecha_creacion as fecha_creacion, 
                        e.descripcion as descripcion
                    FROM Usuarios u 
                    INNER JOIN Estado_Usuario e 
                        ON u.fk_id_estado = e.id_estado";

            using var cnn = _db.GetConnection();
            try
            {
                cnn.Open();
                var cmd = new SqlCommand(query, cnn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListaU.Add(new Usuario
                    {
                        IdUsuario = (int)reader["id_usuario"],
                        UsuarioLg = reader["usuario"].ToString(),
                        Nom_Completo = reader["nom_usuario"].ToString(),
                        Fk_id_estado = (int)reader["fk_id_estado"],
                        Fecha_creacion = (DateTime)reader["fecha_creacion"],
                        Descripcion = reader["descripcion"].ToString()

                    });
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL OBTENER USUARIOS." + ex.Message, ex);

            }
            return ListaU;

        }

        public void InsertarUsuario(Usuario usuario)
        {
            string query = "";
            var contraseniaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);


            
            query = $@"INSERT INTO Usuarios (
                        usuario
                        , nom_usuario
                        , contrasenia
                        , fk_id_estado
                        , fecha_creacion) 
                    VALUES (@Usuario
                        , @NombreU
                        , @Contrasenia
                        , @Estado
                        , GETDATE())";

            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuario.UsuarioLg);
                cmd.Parameters.AddWithValue("@NombreU", usuario.Nom_Completo);
                cmd.Parameters.AddWithValue("@Contrasenia", contraseniaHash);
                cmd.Parameters.AddWithValue("@Estado", usuario.Fk_id_estado);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL INSERTAR USUARIOS." + ex.Message, ex);

            }

        }
        private bool ValidarContraseniaCambio(string ContraseniaCambio, int IdUsuario)
        {
            bool contraseniaCambio = false;
            string contraseniaHashAlmacenada = "";
            string query = $@"SELECT  contrasenia
                                FROM Usuarios 
                               WHERE id_usuario = @Id_Usuario";

            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_Usuario", IdUsuario);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        contraseniaHashAlmacenada = reader["contrasenia"].ToString();
                    }
                }
                contraseniaCambio = !BCrypt.Net.BCrypt.Verify(ContraseniaCambio, contraseniaHashAlmacenada);


            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL VALIDAR CAMBIO DE CONTRASEÑA." + ex.Message, ex);

            }


            return contraseniaCambio;
        }


        public void ActualizarUsuario(Usuario usuario)
        {

            bool CambioContrasenia = false;
            string query = "";



            
            query = $@"UPDATE Usuarios SET 
                        usuario = @UsuarioL
                        , nom_usuario = @Nombre
                        , fk_id_estado = @Estado";
            if (!string.IsNullOrWhiteSpace(usuario.Contrasenia))
            {
                CambioContrasenia = ValidarContraseniaCambio(usuario.Contrasenia, usuario.IdUsuario);
            }

            if (CambioContrasenia)
            {
                query += ", contrasenia = @ContraseniaA";

            }
            query += " WHERE id_usuario = @Id_Usuario";

            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_Usuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@UsuarioL", usuario.UsuarioLg);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nom_Completo);
                cmd.Parameters.AddWithValue("@Estado", usuario.Fk_id_estado);

                if (CambioContrasenia)
                {
                    var nuevaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);
                    cmd.Parameters.AddWithValue("@ContraseniaA", nuevaHash);
                }

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ACTUALIZAR USUARIOS." + ex.Message, ex);

            }
        }


        public void EliminarUsuario(int id)
        {

            string query = "";
    
            query = $@"UPDATE Usuarios SET 
                            fk_id_estado = ";
            query += ESTADO_ELIMINADO;
            query += " WHERE id_usuario = @Id_Usuario";

            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_Usuario", id);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ELIMINAR USUARIOS." + ex.Message, ex);

            }

        }

    }
}
