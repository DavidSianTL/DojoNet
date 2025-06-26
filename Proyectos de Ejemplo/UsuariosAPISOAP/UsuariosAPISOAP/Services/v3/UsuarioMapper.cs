using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Services.v3
{
    public class UsuarioMapper
    {
        public static UsuarioDTO ToDTO(UsuarioEF usuario)
        {
            return new UsuarioDTO
            {
                id_usuario = usuario.id_usuario,
                usuario = usuario.usuario,
                nom_usuario = usuario.nom_usuario,
                fk_id_estado = usuario.fk_id_estado,
                fecha_creacion = usuario.fecha_creacion
            };
        }
    }
}
