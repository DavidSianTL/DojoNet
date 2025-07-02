using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoExpress.Entidades.Models
{
    [Table("Usuarios")]
    public class UsuariosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Column("nombreCompleto")]
        public string NombreCompleto { get; set; }

        [Column("usuario")]
        public string Usuario { get; set; }

        [Column("contrasenia")]
        public string Contrasenia { get; set; }

        [Column("token")]
        public string Token { get; set; }
    }
}