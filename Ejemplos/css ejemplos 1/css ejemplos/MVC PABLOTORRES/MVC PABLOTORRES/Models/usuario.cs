using System.ComponentModel.DataAnnotations;
namespace MVC_PABLOTORRES.Models
{
    public class usuario
    {
        public int id {get;set;}
        [Required (ErrorMessage ="El campo nombre es obligatorio.")]
        [StringLength(50, ErrorMessage ="El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre")]
        public string nombre {get;set; }
        [Required (ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]

        public string Apellido {get;set; }
        [Required (ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]

        public string Correo {get;set; }
        [Required (ErrorMessage = "El campo {0} es obligatorio")]

        public string Direccion {get;set; }
        public string FechaNaciemiento {get;set; }
    }
}
