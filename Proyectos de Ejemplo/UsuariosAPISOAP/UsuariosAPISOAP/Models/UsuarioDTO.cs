using System.Runtime.Serialization;

namespace UsuariosAPISOAP.Models
{
    public class UsuarioDTO
    {
        
        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string nom_usuario { get; set; }
        public int fk_id_estado { get; set; }
        public DateTime fecha_creacion { get; set; }
    }

}
