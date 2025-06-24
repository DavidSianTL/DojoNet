using System.ComponentModel.DataAnnotations;

namespace ApiClinicaMedica.Models
{
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public ICollection<Cita>? Citas { get; set; } = new List<Cita>();

    }
}
