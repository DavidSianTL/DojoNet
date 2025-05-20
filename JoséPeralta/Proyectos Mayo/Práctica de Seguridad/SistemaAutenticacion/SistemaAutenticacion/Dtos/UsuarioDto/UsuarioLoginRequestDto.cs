using System.ComponentModel.DataAnnotations;

namespace SistemaAutenticacion.Dtos.UsuarioDto
{
    public class UsuarioLoginRequestDto
    {


        public string? Email { get; set; }

        // [Required]
        public string? Password { get; set; }

    }
}
