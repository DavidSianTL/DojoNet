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

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "El nombre del rol debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[\p{L}\p{N}\s\-_]+$",
            ErrorMessage = "Solo se permiten letras, números, espacios, guiones y guiones bajos.")]
        [Column("NombreRol")]
        public string NombreRol { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado del rol es obligatorio.")]
        [Column("Estado")]
        public bool Estado { get; set; } = true;

       
}
