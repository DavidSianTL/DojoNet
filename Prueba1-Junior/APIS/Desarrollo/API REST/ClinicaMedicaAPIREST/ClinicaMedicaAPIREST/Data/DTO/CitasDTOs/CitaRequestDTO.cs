namespace ClinicaMedicaAPIREST.Data.DTO.CitasDTOs
{
    public class CitaRequestDTO
    {   
        public int Paciente_Id { get; set; }
        public int Medico_Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
    }
}
