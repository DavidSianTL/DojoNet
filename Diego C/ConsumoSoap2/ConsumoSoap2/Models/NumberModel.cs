using System.ComponentModel.DataAnnotations;

namespace ConsumoSoap2.Models
{
    public class NumberModel
    {
        [Required(ErrorMessage = "el numero es requerido.")]
        [Range(1, 100, ErrorMessage = "el numero debe estar entre 1 y 100.")]
        public int Number { get; set; }
        public string? Result { get; set; }
    }
}
