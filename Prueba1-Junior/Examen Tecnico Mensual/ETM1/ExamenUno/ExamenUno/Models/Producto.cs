using System.ComponentModel.DataAnnotations;

namespace ExamenUno.Models
{
    public class Producto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; } = string.Empty;
        [Required]
        public decimal price { get; set; } = 1;
    }
}
