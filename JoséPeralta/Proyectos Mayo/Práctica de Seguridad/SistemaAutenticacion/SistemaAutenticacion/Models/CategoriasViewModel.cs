using System.ComponentModel.DataAnnotations;

namespace SistemaAutenticacion.Models
{
    public class CategoriasViewModel
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(75, ErrorMessage = "El campo Nombre no puede exceder los 100 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Creación es obligatorio")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        // Navegación: una categoría tiene muchos productos
        public ICollection<ProductosViewModel> Productos { get; set; }

    }
}
