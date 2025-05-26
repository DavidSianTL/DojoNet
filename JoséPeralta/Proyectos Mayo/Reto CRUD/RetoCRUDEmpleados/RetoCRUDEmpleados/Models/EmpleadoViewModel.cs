using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetoCRUDEmpleados.Models
{
    [Table("Empleados")]
    public class EmpleadoViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Puesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo {0} debe ser un número positivo.")]
        public decimal SalarioBase { get; set; }

        public bool Activo { get; set; } = true;
    }
}
