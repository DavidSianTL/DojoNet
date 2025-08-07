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

        [Required(ErrorMessage = "El nombre del sistema es obligatorio.")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "El nombre debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[\p{L}\p{N}\s\.\-&]+$",
            ErrorMessage = "Solo se permiten letras, números, espacios, puntos, guiones y &.")]
        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500,
            ErrorMessage = "La descripción no puede exceder {1} caracteres.")]
        [RegularExpression(@"^[\p{L}\p{N}\s\.,;:()\-]*$",
            ErrorMessage = "Caracteres especiales no permitidos en la descripción.")]
        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código del sistema es obligatorio.")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "El código debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$",
            ErrorMessage = "Solo se permiten letras, números, guiones y guiones bajos.")]
        [Column("Codigo")]
        public string Codigo { get; set; } = string.Empty;


        [Column("FK_IdEstado")]
        public int FK_IdEstado { get; set; }  
        [NotMapped]
        public bool EstadoActivo
        {
            get => FK_IdEstado == 1;
            set => FK_IdEstado = value ? 1 : 2;
        }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato de fecha inválido.")]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

 
    }
}
