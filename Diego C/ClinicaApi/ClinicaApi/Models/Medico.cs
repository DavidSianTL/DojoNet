using ClinicaApi.Models;

public class Medico
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }

    // Relación muchos a muchos
    public ICollection<MedicoEspecialidad> MedicoEspecialidades { get; set; } = new List<MedicoEspecialidad>();

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}