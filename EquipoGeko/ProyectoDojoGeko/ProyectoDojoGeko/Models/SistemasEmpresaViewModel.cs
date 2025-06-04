using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    [Table("SistemasEmpresa")]
    public class SistemasEmpresaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdSistemasEmpresa")]
        public int IdSistemasEmpresa { get; set; }

        [Column("FK_IdEmpresa")]
        public int FK_IdEmpresa { get; set; }

        [Column("FK_IdSistema")]
        public int FK_IdSistema { get; set; }

        // Propiedades de navegación hacia Roles y Permisos
        [ForeignKey("FK_IdEmpresa")]
        public EmpresaViewModel? Empresa { get; set; }

        [ForeignKey("FK_IdSistema")]
        public SistemaViewModel? Sistema { get; set; }

    }
}
