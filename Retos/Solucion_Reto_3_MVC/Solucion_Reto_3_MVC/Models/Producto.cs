using System.ComponentModel.DataAnnotations;

namespace Solucion_Reto_3_MVC.Models
{
    public class Producto
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre del Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Cantidad en stock")]
        public double Stock { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Precio")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha Ingreso")]
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaCaducidad { get; set; }
    }
}
