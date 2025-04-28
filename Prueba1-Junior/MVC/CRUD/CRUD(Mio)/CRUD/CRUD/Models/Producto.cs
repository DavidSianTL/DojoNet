using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Producto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; } = decimal.Zero;
    }
}
