using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;

namespace wbEjercicioEmpleadosMVC.Models
{
    public class EmpleadoModelWS
    {
        public int EmpleadoID { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        [Required]
        public string Puesto { get; set; }
        [Required]
        public decimal SalarioBase { get; set; }
        public bool Activo { get; set; }

    }
}
