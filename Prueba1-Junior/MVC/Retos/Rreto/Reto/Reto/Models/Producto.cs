using System.ComponentModel.DataAnnotations;

namespace Reto.Models
{
    public class Producto
    {
        [Required]
        public int Id { get; set; }
        [Required] 
        public string Nombre { get; set; }
        [Required]
        public decimal Precio{ get; set; }
    }
}
