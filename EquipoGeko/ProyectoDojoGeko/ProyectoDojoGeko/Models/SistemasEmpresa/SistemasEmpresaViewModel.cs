using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.SistemasEmpresa
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
        public EmpresaViewModel? Empresa { get; set; }

        public SistemaViewModel? Sistema { get; set; }

    }
}
