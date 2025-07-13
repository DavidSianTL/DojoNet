using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models
{
    [Table("Cuentas")]
    public class CuentasViewModel
    {
        [Key]
        [Column("IdCuenta")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("TipoCuenta")]
        public string TipoCuenta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("NumeroCuenta")]
        public string NumeroCuenta { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("Saldo")]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdCliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdSucursal")]
        public int SucursalId { get; set; }

        [Column("FechaApertura")]
        public DateTime FechaApertura { get; set; }

    }
}
