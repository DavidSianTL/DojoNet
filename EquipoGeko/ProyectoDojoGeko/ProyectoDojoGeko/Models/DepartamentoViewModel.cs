using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProyectoDojoGeko.Models
{
    [Table("Departamentos")]
    public class DepartamentoViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdDepartamento")]
        public int IdDepartamento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9\s\-]+$", ErrorMessage = "El campo {0} contiene caracteres inválidos.")]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripción no debe exceder los {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9\s\.,;:()\-]*$", ErrorMessage = "La descripción contiene caracteres inválidos.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "El campo {0} solo puede contener letras, números y guiones.")]
        [Column("Codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.DateTime)]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("Estado")]
        public bool Estado { get; set; } = true;
    }
}

