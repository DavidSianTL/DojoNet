using System;
using System.ComponentModel.DataAnnotations;

namespace pruebaWeb.Models
{
    public class EmpleadoModelo
    {
        public int Id { get; set; }
            
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un numero postivo")]

        public string Puesto { get; set; }

        public decimal Salario { get; set; }
        [Required]
        [DataType(DataType.Date)]

        public DateTime FechaContratacion { get; set; }
    }
}
