using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Sistemas")]
    public class SistemaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdSistema")]
        public int IdSistema { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9\s\.\-&]+$", ErrorMessage = "El campo {0} contiene caracteres inválidos.")]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9\s\.,;:()\-]*$", ErrorMessage = "El campo {0} contiene caracteres inválidos.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "El campo {0} solo puede contener letras, números y guiones.")]
        [Column("Codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Column("Estado")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.DateTime, ErrorMessage = "El campo {0} debe ser una fecha válida.")]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
