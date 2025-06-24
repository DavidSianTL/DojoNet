using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trabajo_APIRest.Models
{
    [Table("Usuarios")]
    public class UsuarioViewModel
    {
        [Key]
        [Column("idUsuario")]
        public int? IdUsuario { get; set; }

        [Column("nombreCompleto")]
        public string? NombreCompleto { get; set; }

        [Column("usuario")]
        public string? Usuario { get; set; }

        [Column("contrasenia")]
        public string? Contrasenia { get; set; }

        [Column("token")]
        public string? Token { get; set; }
    }
}
