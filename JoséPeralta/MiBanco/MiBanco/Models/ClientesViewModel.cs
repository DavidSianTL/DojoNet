using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models
{
    [Table("Clientes")]
    public class ClientesViewModel
    {
        [Key]
        [Display(Name = "Id Cliente")]
        [Column("IdCliente")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombres")]
        [Column("Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Apellidos")]
        [Column("Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombre Completo")]
        [Column("NombreCompleto")]
        public string NombreCompleto
        {
            // Obtenemos el nombre completo y se lo pasamos al atributo
            get { return ObtenerNombreCompleto(); }
        }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "DPI")]
        [Column("DPI")]
        public string DPI { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Teléfono")]
        [Column("Telefono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Correo Personal")]
        [Column("CorreoPersonal")]
        public string CorreoPersonal { get; set; }


        // Función que obtiene el nombre completo
        public string ObtenerNombreCompleto()
        {
            return Nombres + " " + Apellidos;
        }
    }
}
