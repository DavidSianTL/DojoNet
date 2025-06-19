namespace AuthenticationSystem.Models
{
    public class PermisoRol
    {
        public string IdRol { get; set; } = string.Empty;
        public int IdPermiso { get; set; } 



        // Propiedades de navegación para las relaciones entre Rol y Permiso. 
        public Rol rol { get; set; } = null!;  
        public Permiso permiso { get; set; } = null!;
    }
}
