using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAutenticacion.Models
{
    public class ProductosViewModel
    {
        [Key]
        public Guid IdProducto { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(75, ErrorMessage = "El campo Nombre no puede exceder los 100 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El campo Descripción es obligatorio")]
        [StringLength(200, ErrorMessage = "El campo Descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo Precio debe ser mayor que cero")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo Stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El campo Stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Creación es obligatorio")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        // FK
        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        // Navegación
        public CategoriasViewModel Categoria { get; set; }
    }
}
