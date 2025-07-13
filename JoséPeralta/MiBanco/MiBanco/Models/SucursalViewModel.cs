using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models{

    [Table("Sucursales")]
    public class SucursalViewModel{

        [Key]
        [Column("IdSucursal")]
        public int Id {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Nombre")]
        public string Nombre {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Direccion")]
        public string Direccion {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Telefono")]
        public string Telefono {get; set;}

        [Column("Correo")]
        public string? Correo {get; set;}
    }
}