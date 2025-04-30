using System.ComponentModel.DataAnnotations;

namespace Examen_mes_abril.Models
{
    public class ProductoViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreProducto { get; set; }

        [Required]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Required]
        public string Precio { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [MaxLength(3)]
        public string Talla { get; set; }

        [Required]
        public string Color { get; set; }

    }
}
