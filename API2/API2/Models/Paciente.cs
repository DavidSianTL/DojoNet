using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API2.Models;

namespace API2.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        // Relación con Citas
        public ICollection<Cita> Citas { get; set; }
    }
}
