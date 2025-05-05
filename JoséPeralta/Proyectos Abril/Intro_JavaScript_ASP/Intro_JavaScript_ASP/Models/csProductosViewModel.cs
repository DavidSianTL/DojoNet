using System.ComponentModel.DataAnnotations;

namespace Intro_JavaScript_ASP.Models
{
    public class csProductosViewModel
    {
        [Required]
        public string Producto { get; set; }

        [Required]

        public int Cantidad { get; set; }

        [Required]

        public decimal Precio { get; set; }

        [Required]

        public decimal Total { get; set; }

        [Required]

        public DateTime FechaCompra { get; set; }

        [Required]

        public string Ubicacion { get; set; }

    }
}
