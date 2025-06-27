using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosAPISOAP.Models
{
    [DataContract]
    public class UsuarioEF
    {
       
        [Key] // Esta línea es clave
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 0)]
        [Column("id_usuario")]
        public int id_usuario { get; set; }

      
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [MaxLength(50)]
        [DataMember(Order = 1)]
        [Column("usuario")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [MaxLength(100)]
        [DataMember(Order = 2)]
        [Column("nom_usuario")]
        public string nom_usuario { get; set; }

        [MaxLength(100)]
        [DataMember(Order = 3)]
        [Column("contrasenia")]
        public string contrasenia { get; set; }

        [Required]
        [DataMember(Order = 4)]
        [Column("fk_id_estado")]
        public int fk_id_estado { get; set; }

        [DataMember(Order = 5)]
        [Column("fecha_creacion")]
        public DateTime fecha_creacion { get; set; } = DateTime.Now;
    }
}
