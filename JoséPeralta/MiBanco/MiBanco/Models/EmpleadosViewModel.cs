using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models{

    [Table("Empleados")]
    public class EmpleadosViewModel{

        [Key]
        [Column("IdEmpleado")]
        public int Id {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Nombres")]
        public string Nombres {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Apellidos")]
        public string Apellidos {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("NombreCompleto")]
        public string NombreCompleto
        {
            // Obtenemos el nombre completo y se lo pasamos al atributo
            get { return ObtenerNombreCompleto(); }
        }

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("DPI")]
        public string DPI {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("Telefono")]
        public string Telefono {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("CorreoPersonal")]
        public string CorreoPersonal {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("FK_IdRol")]
        public int RolId {get; set;}

        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [Column("FK_IdSucursal")]
        public int SucursalId {get; set;}

        // Función que obtiene el nombre completo
        public string ObtenerNombreCompleto()
        {
            return Nombres + " " + Apellidos;
        }

    }
}