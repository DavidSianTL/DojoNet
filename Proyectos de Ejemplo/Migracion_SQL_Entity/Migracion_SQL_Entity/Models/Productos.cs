using System.ComponentModel.DataAnnotations;

namespace Migracion_SQL_Entity.Models
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; } = decimal.Zero;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; } = 0;

        [Required]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres.")]
        public string Categoria { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        // Constructor por defecto
        public Productos()
        {
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }
    }
}
