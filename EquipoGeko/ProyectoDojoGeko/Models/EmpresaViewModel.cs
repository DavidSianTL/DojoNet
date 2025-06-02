using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("Empresas")]
    public class EmpresaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEmpresa")]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
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
