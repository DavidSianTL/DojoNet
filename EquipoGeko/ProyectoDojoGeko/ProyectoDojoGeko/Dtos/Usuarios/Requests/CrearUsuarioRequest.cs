using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Dtos.Usuarios.Requests
{
    public class CrearUsuarioRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de {1} caracteres.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del empleado es obligatorio.")]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        public int EstadoId { get; set; } = 1; // Valor por defecto para estado activo
    }
}
