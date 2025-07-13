using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EvaluacionApi.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Dpi es obligatorio")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El Dpi debe tener 13 digitos")]
        public string Dpi { get; set; }

        [Required (ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        [StringLength(100, ErrorMessage = "La descripcion no puede ser mayor a 100 caractares")]
        public string Descripcion { get; set; } 
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
