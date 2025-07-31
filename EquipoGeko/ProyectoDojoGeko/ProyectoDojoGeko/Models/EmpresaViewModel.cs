using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Empresas")]
    public class EmpresaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEmpresa")]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(150, MinimumLength = 3,
            ErrorMessage = "El nombre debe tener entre {2} y {1} caracteres.")]
        //[RegularExpression(@"^[\p{L}\p{N}\s\.\-&'()]+$",
            //ErrorMessage = "Solo se permiten letras, números, espacios y los caracteres especiales .-&'()")]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500,
            ErrorMessage = "La descripción no puede exceder los {1} caracteres.")]
        [RegularExpression(@"^[\p{L}\p{N}\s\.,;:()\-/&'""]*$",
            ErrorMessage = "Contiene caracteres especiales no permitidos.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código de empresa es obligatorio.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El código debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Z0-9\-_]+$", ErrorMessage = "Solo mayúsculas, números, guiones y guiones bajos.")]
        [Column("Codigo")]
        public string Codigo { get; set; } = string.Empty;

        public string? Logo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado de la empresa es obligatorio.")]
        [Column("FK_IdEstado")]
        public int FK_IdEstado { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.DateTime)]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        
    }
}
