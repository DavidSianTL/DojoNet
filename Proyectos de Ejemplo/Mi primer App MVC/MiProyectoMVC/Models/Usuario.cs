using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ser un correo válido.")]
    [DataType(DataType.EmailAddress)]
    public string Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Contraseña { get; set; }
}
