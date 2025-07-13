using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiBanco.Models{

    [Table("Bitacora")]
    public class BitacoraViewModel{

        [Key]
        [Column("IdBitacora")]
        public int Id {get; set;}

        [Column("Accion")]
        public string Accion {get; set;}

        [Column("Descripcion")]
        public string Descripcion {get; set;}

        [Column("FechaBitacora")]
        public DateTime FechaBitacora {get; set;}

    }
}