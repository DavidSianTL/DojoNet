using System.ComponentModel.DataAnnotations;

namespace wbSistemaSeguridadMVC.Models
{
    public class Token
    {

        public int IdToken { get; set; }
        public int IdUsuario { get; set; }  
        public int IdSistema { get; set; }
        public string TokenS { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int Estado { get; set; }
       
        
    }
}
