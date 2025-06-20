using System.ComponentModel.DataAnnotations;
namespace ClinicaApi.Models;

public class Cita
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un médico")]
    public int MedicoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un paciente")]
    public int PacienteId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    public string Hora { get; set; }
}
