using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("EquiposProyecto")]
    public class EquipoProyectoViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEquipoProyecto")]
        public int IdEquipoProyecto { get; set; }

        [ForeignKey("Proyecto")]
        [Column("FK_IdProyecto")]
        public int FK_IdProyecto { get; set; }

        [ForeignKey("Equipo")]
        [Column("FK_IdEquipo")]
        public int FK_IdEquipo { get; set; }

        // Propiedades de navegación
        public virtual ProyectoViewModel? Proyecto { get; set; }
        public virtual EquipoViewModel? Equipo { get; set; }
    }
}