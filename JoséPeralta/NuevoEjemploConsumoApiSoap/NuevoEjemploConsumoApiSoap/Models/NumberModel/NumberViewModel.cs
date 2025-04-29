using System.ComponentModel.DataAnnotations;

namespace NuevoEjemploConsumoApiSoap.Models.NumberModel
{
    public class NumberViewModel
    {

        [Required]
        [Range(1, 100, ErrorMessage = "El número debe estar entre 1 y 100.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número debe ser un valor numérico.")]
        [StringLength(3, ErrorMessage = "El número debe tener un máximo de 3 dígitos.")]
        public int Number { get; set; }

        public string? Result { get; set; }

    }
}
