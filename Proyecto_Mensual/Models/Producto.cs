using System;
using System.ComponentModel.DataAnnotations;

namespace AppFuturista.Models
{
    public class Producto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        
        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }
        
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
        
        [Display(Name = "Imagen URL")]
        public string ImagenUrl { get; set; } = "/img/producto-default.png";
        
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        [Display(Name = "Última Actualización")]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
