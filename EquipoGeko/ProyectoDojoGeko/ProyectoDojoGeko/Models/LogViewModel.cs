using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Logs")]
    public class LogViewModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdLog")]
        public int IdLog { get; set; }

        [DataType(DataType.DateTime)]
        [Column("FechaEntrada")]
        public DateTime FechaEntrada { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(75, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Accion")]
        public string Accion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;


        [Column("Estado")]
        public bool Estado { get; set; } = true;

    }
}
