using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClinicaAPI.Models;

namespace ClinicaAPI.Models
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Propiedad de navegación inversa
        public ICollection<Medico> Medicos { get; set; }
    }
}
