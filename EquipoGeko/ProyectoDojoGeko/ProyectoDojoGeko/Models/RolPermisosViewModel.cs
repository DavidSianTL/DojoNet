using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    public class RolPermisosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("IdRolPermiso")]
        public int IdRolPermiso { get; set; } 
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdRol")]
        public int FK_IdRol { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdPermiso")]
        public int FK_IdPermiso { get; set; }

        [ForeignKey("FK_IdRol")]
        public RolesViewModel? Rol { get; set; }

        [ForeignKey("FK_IdPermiso")]
        public PermisoViewModel? Permiso { get; set; }

    }
}
