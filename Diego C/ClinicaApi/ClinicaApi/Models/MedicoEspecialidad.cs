namespace ClinicaApi.Models
{
    public class MedicoEspecialidad
    {
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }
    }
}
