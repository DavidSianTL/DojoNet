namespace wbSistemaSeguridadMVC.Models
{
    public class UsuarioT
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; }  
        public string NombreCompleto { get; set; } 
        public string Contrasenia { get; set; }
        public int Estado { get; set; }
    }
}
