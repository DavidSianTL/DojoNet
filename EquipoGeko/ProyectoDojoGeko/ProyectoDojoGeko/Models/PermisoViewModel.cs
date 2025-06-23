using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Permisos")]
    public class PermisoViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdPermiso")]
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "El nombre del permiso es obligatorio.")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "El nombre debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[\p{L}\p{N}\s\-_\.]+$",
            ErrorMessage = "Solo se permiten letras, números, espacios, guiones, puntos y guiones bajos.")]
        [Column("NombrePermiso")]
        public string NombrePermiso { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción del permiso es obligatoria.")]
        [StringLength(150, MinimumLength = 10,
            ErrorMessage = "La descripción debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[\p{L}\p{N}\s\.,;:\-_\(\)\/]+$",
            ErrorMessage = "Descripción contiene caracteres no permitidos.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado del permiso es obligatorio.")]
        [Column("Estado")]
        public bool Estado { get; set; } = true;

  
    }
}