using System.ComponentModel.DataAnnotations;

namespace Proyercto1.Models
{
    public class Producto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")]
        public string Nombre { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }
        [Required]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$")]
        public string Descripcion { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public string Stock { get; set; }

    }
}
