using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoExpress.Entidades.Models
{
    public class TipoVehiculosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipoVehiculo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
    }
}
