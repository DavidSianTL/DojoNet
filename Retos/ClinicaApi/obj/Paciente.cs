using System.ComponentModel.DataAnnotations;

namespace ClinicaApi.Models;

public class Paciente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del paciente es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; }

    [EmailAddress(ErrorMessage = "Correo inválido")]
    [Required(ErrorMessage = "El correo es obligatorio")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; }
}
