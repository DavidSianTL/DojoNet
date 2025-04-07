using System.ComponentModel.DataAnnotations;

namespace ValidacionesCliente.Models
{
    public class Produco
    {
        [Required]
        public int Id { get; set; }
        public string Nombre {get; set;}
        [Required]
        public string  Descripcion {get; set;}
        [Required]
        public DateTime  FechaIngreso {get; set;}
        [Required]
        public string  Tipo {get; set;}
        [Required]
        public string Color {get; set;}
        [Required]
        public double Size{get; set;}
        public int Cantidad { get; set; }
    }
}
