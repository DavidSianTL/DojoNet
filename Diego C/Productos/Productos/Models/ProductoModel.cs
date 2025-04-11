using System.ComponentModel.DataAnnotations;

namespace Productos.Models
{
    public class ProductoModel
    {
        

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        [Display(Name = "Nombre del Producto")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres.")]
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Display(Name = "Precio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El formato del precio es inválido. Solo se permiten hasta dos decimales.")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Display(Name = "Cantidad")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre 1 y 1000.")]
        [RegularExpression(@"^[1-9][0-9]{0,2}$", ErrorMessage = "Solo se permiten números enteros positivos.")]
        public int? Cantidad { get; set; }
    }
}