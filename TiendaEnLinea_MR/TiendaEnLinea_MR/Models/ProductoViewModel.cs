using System.ComponentModel.DataAnnotations;

namespace TiendaEnLinea_MR.Models
{
    public class ProductoViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string NombreProducto { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public int Cantidad { get; set; }


    }
}
