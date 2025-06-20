using System.ComponentModel.DataAnnotations;

namespace SistemaAutenticacion.Dtos.UsuarioDto
{
    public class UsuarioLoginRequestDto
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Email { get; set; }

        // [Required]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Password { get; set; }

    }
}
