using System;
using System.ComponentModel.DataAnnotations;
namespace css_ejemplos.Models
{
    public class Class
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Puesto { get; set; }
        [Required]

        [Range(0, double.MaxValue, ErrorMessage = "El salario debe de ser un numero positivo")]
        public decimal salario { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Fechacontratacion
        { get; set; }
    }
}
