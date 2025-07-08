using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Estados")]
    public class EstadosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("Estado")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(75, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("Activo")]
        public bool Activo { get; set; }

    }
}
