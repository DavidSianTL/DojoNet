using System.ComponentModel.DataAnnotations;
namespace MVC_CRUD_INTRODUCCION.Models
{
    public class ProyectoViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Nombre del Proyecto")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El campo {0} solo puede contener letras")]
        public string NombreProyecto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Descripción del Proyecto")]
        public string Descripcion { get; set; }

        public string? Estado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Fin")]
        public DateTime FechaFin { get; set; }


    }
}
