using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Equipos")]
    public class EquipoViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEquipo")]
        public int IdEquipo { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "El nombre del equipo debe tener entre {2} y {1} caracteres.")]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [ForeignKey("Estado")]
        [Column("FK_IdEstado")]
        public int FK_IdEstado { get; set; } = 1;

        
    }
}
