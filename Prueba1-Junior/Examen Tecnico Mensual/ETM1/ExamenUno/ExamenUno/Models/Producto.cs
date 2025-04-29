using System.ComponentModel.DataAnnotations;

namespace ExamenUno.Models
{
    public class Producto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public decimal price { get; set; } 
    }
}
