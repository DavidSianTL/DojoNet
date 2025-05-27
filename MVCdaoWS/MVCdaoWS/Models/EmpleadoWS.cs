using System;
using System.ComponentModel.DataAnnotations;

namespace MVCdaoWS.Models
{
    public class EmpleadoWS
    {
        public int EmpleadoID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }

        [Required]
        [StringLength(50)]
        public string Puesto { get; set; }

        public decimal SalarioBase { get; set; }
        public bool Activo { get; set; }
    }
}
