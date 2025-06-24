using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API2.Models
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Relación con Médicos
        public ICollection<Medico> Medicos { get; set; }
    }
}
