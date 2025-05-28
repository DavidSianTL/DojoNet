using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Permisos")]
    public class PermisosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdPermiso")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("NombrePermiso")]
        public string NombrePermiso { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El campo {0} debe tener entre {5} y {50} caracteres.")]
        [Column("NombrePermiso")]
        public string Descripcion { get; set; } = string.Empty;


    }
}
