using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Models
{
    public class ModuloViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public int IdSistema { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio.")]
        public int FK_IdEstado { get; set; }
    }
}
