namespace ClinicaApi.Models
{
    public class Especialidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<Medico> Medicos { get; set; }
    }
}
