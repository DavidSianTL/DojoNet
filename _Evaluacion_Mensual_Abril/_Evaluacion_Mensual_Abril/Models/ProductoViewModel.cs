using System.ComponentModel.DataAnnotations;

namespace Evaluacion_Mensual_Abril.Models
{
    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no debe superar los 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50)]
        public string Categoria { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe proporcionar una URL de imagen.")]
        [Url(ErrorMessage = "Debe ser una URL válida.")]
        public string Imagen { get; set; }
    }
}
