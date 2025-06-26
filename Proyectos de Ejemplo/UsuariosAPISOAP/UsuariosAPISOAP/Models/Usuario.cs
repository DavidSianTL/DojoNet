using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace UsuariosAPISOAP.Models
{
    [DataContract]
    public class Usuario
    {
        [DataMember]
        public int IdUsuario { get; set; }

        [DataMember]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string UsuarioLg { get; set; }

        [DataMember]
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string Nom_Completo { get; set; }

        [DataMember]
        public string Contrasenia { get; set; }

        [DataMember]
        [Required]
        public int Fk_id_estado { get; set; }

        [DataMember]
        public DateTime Fecha_creacion { get; set; }

        [DataMember]
        public string Descripcion { get; set; }
    }
}
