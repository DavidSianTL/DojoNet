using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EvaluacionApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }


        [Required(ErrorMessage = "El Dpi es obligatorio")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El Dpi debe tener 13 digitos")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El Dpi solo debe contener numeros")]
        public string Dpi { get; set; }


        [Required(ErrorMessage = "El telefono es obligatorio")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El telfono solo debe contener numeros")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "El saldo es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo no puede ser negativo")]
        public decimal Saldo { get; set; }


    }
}
