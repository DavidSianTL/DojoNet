using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.Usuario
{
    [Table("UsuarioToken")]
    public class TokenUsuarioViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdUsuarioToken")]
        public int IdUsuarioToken { get; set; }

        [DataType(DataType.DateTime)]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("Token")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.DateTime)]
        [Column("TiempoExpira")]
        public DateTime TiempoExpira { get; set; }

        [Column("FK_IdUsuario")]
        public int FK_IdUsuario { get; set; }

        // Propiedad de navegación
        [ForeignKey("FK_IdUsuario")]
        // Aquí relacionamos el token con un usuario específico
        public virtual UsuarioViewModel? Usuario { get; set; }

    }
}
