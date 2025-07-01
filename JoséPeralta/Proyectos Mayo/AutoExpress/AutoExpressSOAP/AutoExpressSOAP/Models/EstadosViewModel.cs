using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoExpressSOAP.Models
{
    public class EstadosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstado { get; set; }
        public string EstadoNombre { get; set; }
        public string Descripcion { get; set; }
    }
}
