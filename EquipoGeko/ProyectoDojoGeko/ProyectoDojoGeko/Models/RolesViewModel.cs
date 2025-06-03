using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Roles")]
    public class RolesViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdRol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("NombreRol")]
        public string NombreRol { get; set; } = string.Empty;

        [Column("Estado")]
        public bool Estado { get; set; } = true;
    }
}
