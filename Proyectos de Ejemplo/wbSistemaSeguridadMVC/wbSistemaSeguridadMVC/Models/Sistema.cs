using System.ComponentModel.DataAnnotations;

namespace wbSistemaSeguridadMVC.Models
{
    public class Sistema
    {
        public int IdSistema {  get; set; }

        [Required]
        [StringLength(100)]
        public string NombreSistema { get; set; }
        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; } 


    }
}
