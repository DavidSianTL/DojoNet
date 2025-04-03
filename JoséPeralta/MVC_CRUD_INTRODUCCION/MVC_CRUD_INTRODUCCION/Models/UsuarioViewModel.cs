using System.ComponentModel.DataAnnotations;

namespace MVC_CRUD_INTRODUCCION.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        // Por simplicidad, se omiten las validaciones de los campos
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        // Aquí usando {0} se refiere al nombre del campo, en este caso "Nombre"
        // y {1} se refiere al valor que se le pasa a StringLength
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Display(Name = "Nombre Empleado")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo válida")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Teléfono")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El campo {0} debe contener exactamente 10 dígitos.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Puesto { get; set; }

        public string Direccion { get; set; }

        public string Estado { get; set; }


    }
}
