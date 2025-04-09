using System.ComponentModel.DataAnnotations;

namespace Reto.Models.Producto
{
    public class ProductoModel
    {
        [Required]
        public string Nombre { get; set; }
        [Required] 
        public string Company { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
