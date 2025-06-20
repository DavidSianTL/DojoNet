using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClinicaApi.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }

        
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }

}
