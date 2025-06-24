using System.ComponentModel.DataAnnotations;

namespace ApiClinicaMedica.Models
{
    public class Medico
    {
        [Key]
        public int IdMedico { get; set; }

        public string Nombre { get; set; }  

        public string Email { get; set; }

        public int EspecialidadId { get; set; }

        public Especialidad? Especialidad { get; set; }

        public ICollection<Cita>? Citas { get; set; }
    }
}
