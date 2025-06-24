using System.ComponentModel.DataAnnotations;
namespace ClinicaApi.Models;

public class Medico
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del medico es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; }

    [EmailAddress(ErrorMessage = "El email del medico no es valido")]
    [Required(ErrorMessage = "El email es obligatorio")]
    public string Email { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La especialidad es obligatoria")]
    public int EspecialidadId { get; set; }

    [Required(ErrorMessage = "El telfono es obligatorio")]
	[Phone(ErrorMessage = "Formato de telefono invalido")]
    public string Telefono { get; set; } 
}
