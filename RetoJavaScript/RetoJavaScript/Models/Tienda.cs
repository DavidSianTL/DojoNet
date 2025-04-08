using System.ComponentModel.DataAnnotations;

namespace RetoJavaScript.Models
{
    public class Tienda

    {
        [Required]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")]
        public string? Producto { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double? Precio { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? FechaDeVencimiento { get; set; }

        [Required]
        [EmailAddress]
        public string? correo { get; set; }
        [Required]
        public int? CategoriaId { get; set; }

    }
}

