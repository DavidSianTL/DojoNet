using System.ComponentModel.DataAnnotations;

namespace CRUDEmpleadoImagenes.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio")]
        [StringLength(50)]
        public string Cargo { get; set; }

        public byte[] Foto { get; set; }
        public byte[] Firma { get; set; }
        public byte[] DocumentoPDF { get; set; }
    }
}
