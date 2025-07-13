using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models
{
    
    [Table("Roles")]
    public class RolesViewModel
    {
        [Key]
        [Column("IdRol")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("TipoRol")]
        public string TipoRol { get; set; }
    }

}
