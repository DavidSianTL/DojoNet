using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Producto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
