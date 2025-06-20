using System.ComponentModel.DataAnnotations;

namespace ApiClinicaMedica.Models
{
    public class Especialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }

        public ICollection<Medico> Medicos { get; set; }
    }
}
