using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosApi.Models
{
    public class UsuarioEF
    {
        [Key] // Esta línea es clave
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("usuario")]
        public string UsuarioLg { get; set; }

        [Column("nom_usuario")]
        public string Nom_Completo { get; set; }

        [Column("contrasenia")]
        public string Contrasenia { get; set; }

        [Column("fk_id_estado")]
        public int Fk_id_estado { get; set; }

        [Column("fecha_creacion")]
        public DateTime Fecha_creacion { get; set; }

    }
}
