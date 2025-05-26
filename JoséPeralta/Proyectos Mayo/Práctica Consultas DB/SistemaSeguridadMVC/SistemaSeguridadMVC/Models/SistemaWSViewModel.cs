using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaSeguridadMVC.Models
{
    // Esta clase representa el modelo de vista para los sistemas
    [Table("Sistemas")]
    public class SistemaWSViewModel
    {

        [Key]
        [Display(Name = "Identificador del Sistema")]
        public int IdSistema { get; set; }

        [Display(Name = "Sistema")]
        public string NombreSistema { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string DescripcionSistema { get; set; } = string.Empty;

        [Display(Name = "Empresa asociada")]
        public int IdEmpresa { get; set; }


    }
}
