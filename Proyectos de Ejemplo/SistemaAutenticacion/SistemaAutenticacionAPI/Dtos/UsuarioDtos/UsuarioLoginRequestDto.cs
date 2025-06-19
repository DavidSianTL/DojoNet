using System.ComponentModel.DataAnnotations;

namespace SistemaAutenticacionAPI.Dtos.UsuarioDtos
{
    /// <summary>
    /// Manejo de datos enviados desde el frontend
    /// </summary>
    public class UsuarioLoginRequestDto
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Password { get; set; }
    }
}
