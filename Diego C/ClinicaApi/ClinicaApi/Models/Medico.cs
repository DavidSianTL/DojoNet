namespace ClinicaApi.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }

        public ICollection<Cita> Citas { get; set; }
    }

}
