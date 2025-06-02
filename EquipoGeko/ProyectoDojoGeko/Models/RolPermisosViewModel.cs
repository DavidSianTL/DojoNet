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

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdSistema")]
        public int FK_IdSistema { get; set; }


        // Propiedades de navegación hacia Roles y Permisos
        [ForeignKey("FK_IdRol")]
        public RolesViewModel? Rol { get; set; }

        [ForeignKey("FK_IdPermiso")]
        public PermisoViewModel? Permiso { get; set; }

        [ForeignKey("FK_IdSistema")]
        public SistemaViewModel? Sistema { get; set; }

    }
}
