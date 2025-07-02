using System.ComponentModel.DataAnnotations;
namespace MVC_CRUD_INTRODUCCION.Models
{
    public class EstadoViewModel
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Id del Estado")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El campo {0} solo puede contener letras")]
        [Display(Name = "Nombre del Estado")]
        public string NombreEstado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Descripción del Estado")]
        public string Descripcion { get; set; }


    }
}
