using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models{

    [Table("Usuarios")]
    public class UsuariosViewModel{

        [Key]
        [Column("IdUsuario")]
        public int Id {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Usuario")]
        public string Usuario {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Contrasenia")]
        public string Contraseña {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Token")]
        public string Token {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("FK_IdRol")]
        public int RolId {get; set;}

    }
}