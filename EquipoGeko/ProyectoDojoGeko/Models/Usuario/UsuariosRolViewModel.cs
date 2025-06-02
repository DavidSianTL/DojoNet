using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.Usuario
{
    public class UsuariosRolViewModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public int IdUsuarioRol { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("FK_IdUsuario")]
        public int FK_IdUsuario { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("FK_IdRol")]
        public int FK_IdRol { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("FK_IdSistema")]
        public int FK_IdSistema { get; set; }


        // Propiedades de navegación
        [ForeignKey("FK_IdUsuario")]
        public UsuarioViewModel? Usuario { get; set; }

        [ForeignKey("FK_IdRol")]
        public RolesViewModel? Rol { get; set; }

        [ForeignKey("FK_IdSistema")]
        public SistemaViewModel? Sistema { get; set; }
    }
}
