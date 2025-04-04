using System.ComponentModel.DataAnnotations;

public class UsuarioModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ser un correo válido.")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [Display(Name = "Contraseña")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Contrasena { get; set; }
}
