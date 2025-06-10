using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Sistemas")]
    public class SistemaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdSistema")]
        public int IdSistema { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, MinimumLength = 3)]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "El campo {0} solo debe contener letras y números.")]
        [Column("Codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Column("Estado")]
        public bool Estado { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}