using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Bitacora")]
    public class BitacoraViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdBitacora")]
        public int IdBitacora { get; set; }

        [DataType(DataType.DateTime)]
        [Column("FechaEntrada")]
        public DateTime FechaEntrada { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [ForeignKey("UsuarioViewModel")]
        [Column("FK_IdUsuario")]
        public int FK_IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [ForeignKey("SistemaViewModel")]
        [Column("FK_IdSistema")]
        public int FK_IdSistema { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(75, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Accion")]
        public string Accion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
