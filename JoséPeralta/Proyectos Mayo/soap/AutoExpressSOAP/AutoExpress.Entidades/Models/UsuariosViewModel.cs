using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoExpress.Entidades.Models
{
    [Table("Usuarios")]
    public class UsuariosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Column("nombreCompleto")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre completo no puede tener más de 50 caracteres.")]
        public string NombreCompleto { get; set; }

        [Column("usuario")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(25, ErrorMessage = "El nombre de usuario no puede tener más de 25 caracteres.")]
        public string Usuario { get; set; }

        [Column("contrasenia")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasenia { get; set; }

        [Column("token")]
        public string Token { get; set; }
    }
}
