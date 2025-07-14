using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models{

    [Table("Pagos")]
    public class PagosViewModel{

        [Key]
        [Column("IdPago")]
        public int Id {get; set;}

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("TipoPago")]
        public string TipoPago { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("Monto")]
        public decimal Monto {get; set;}

        [Column("Descripcion")]
        public string? Descripcion {get; set;}

        [Column("FK_NumeroCuentaOrigen")]
        public string CuentaOrigen { get; set;}

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("FK_NumeroCuentaDestino")]
        public string CuentaDestino { get; set; }

        [Column("FK_IdEmpleado")]
        public int? EmpleadoId { get; set; }

        [Column("FK_IdCliente")]
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Column("FechaPago")]
        public DateTime FechaPago {get; set;}

    }   
}