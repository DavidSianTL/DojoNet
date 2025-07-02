using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoExpress.Entidades.Models
{
    [Table("TipoVehiculo")]
    public class TipoVehiculosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipoVehiculo { get; set; }

        [Required(ErrorMessage = "El campo 'Tipo' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El tipo no puede exceder los 100 caracteres.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres.")]
        public string Descripcion { get; set; }
    }
}
