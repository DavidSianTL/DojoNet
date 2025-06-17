namespace AuthenticationSystem.Models
{
    public class PermisoRol
    {
        public string IdRol { get; set; } = string.Empty;
        public string IdPermiso { get; set; } = string.Empty;



        // Propiedades de navegación para las relaciones entre Rol y Permiso. 
        public Rol rol { get; set; } = null!;  
        public Permiso permiso { get; set; } = null!;
    }
}
