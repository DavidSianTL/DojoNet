using System;
using System.ComponentModel.DataAnnotations;

namespace WbPruebaCSS.Models
{
    public class Empleado_Modeller
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre del Empleado")]
        public string Nombre { get; set; }

        [Required]
        public string Puesto { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un número positivo.")]
        public decimal Salario { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }
    }
}
