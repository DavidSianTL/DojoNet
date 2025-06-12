using Microsoft.Data.SqlClient;
using UsuariosApi.Models;
using System.Threading.Tasks;
using UsuariosApi.Data;


namespace UsuariosApi.DAO
{
    public class daoAuditoria
    {

        private readonly DbConnection _db;

        public daoAuditoria(DbConnection db)
        {
            _db = db;
        }

        public async Task InsertarAuditoriaAsync(Auditoria auditoria)
        {
            string query = @"INSERT INTO Auditoria 
                            (Usuario, Metodo, Ruta, IpOrigen, Estado, Mensaje, Cuerpo, TipoAccion) 
                            VALUES (@Usuario, @Metodo, @Ruta, @IpOrigen, @Estado, @Mensaje, @Cuerpo, @TipoAccion)";

            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Usuario", auditoria.Usuario ?? "Anónimo");
            cmd.Parameters.AddWithValue("@Metodo", auditoria.Metodo);
            cmd.Parameters.AddWithValue("@Ruta", auditoria.Ruta);
            cmd.Parameters.AddWithValue("@IpOrigen", auditoria.IpOrigen);
            cmd.Parameters.AddWithValue("@Estado", auditoria.Estado);
            cmd.Parameters.AddWithValue("@Mensaje", auditoria.Mensaje ?? "");
            cmd.Parameters.AddWithValue("@Cuerpo", (object)auditoria.Cuerpo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TipoAccion", (object)auditoria.TipoAccion ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

    }
}
