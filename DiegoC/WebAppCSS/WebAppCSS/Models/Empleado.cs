
using System.ComponentModel.DataAnnotations;

namespace WebAppCSS.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        public string Puesto { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe de ser un numero positivo")]
        public decimal Salario { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string FechaContratacion { get; set; }
    }
}
