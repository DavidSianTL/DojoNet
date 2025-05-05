using System.ComponentModel.DataAnnotations;

namespace Intro_JavaScript_ASP.Models
{
    public class csUsuarioViewModel
    {
        [Required]
        public string? Nombre { get; set; }

        [Required]

        public string? Apellido { get; set; }

        [Required]

        public int Edad { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo electrónico válida.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El campo {0} no es una dirección de correo electrónico válida.")]
        [Display(Name = "Correo Electrónico")]
        public string? Correo { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
