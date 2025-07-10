using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Dtos.Usuarios.Requests
{
    public class ActualizarUsuarioRequest
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de {1} caracteres.")]
        public string Username { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La contraseña no puede tener más de {1} caracteres.", MinimumLength = 8)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmarPassword { get; set; }

        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        public int EstadoId { get; set; }
    }
}
