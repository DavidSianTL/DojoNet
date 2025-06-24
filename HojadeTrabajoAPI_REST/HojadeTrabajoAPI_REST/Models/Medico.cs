namespace HojadeTrabajoAPI_REST.Models
{
    public class Medico
    {
        public int IdMedico { get; set; }
        public string Nombre { get; set; }
        public int FK_Id_Especialidad { get; set; }
        public string Email { get; set; }
    }
}
