using ClinicaApi.Models;

public class Especialidad
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    // Relación muchos a muchos
    public ICollection<MedicoEspecialidad> MedicoEspecialidades { get; set; } = new List<MedicoEspecialidad>();
}