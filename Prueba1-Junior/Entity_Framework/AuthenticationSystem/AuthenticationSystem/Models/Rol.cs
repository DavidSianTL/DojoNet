using Microsoft.AspNetCore.Identity;

namespace AuthenticationSystem.Models
{
    public class Rol : IdentityRole
    {
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


        // Propiedad de navegación: un rol puede tener varios Permisos.
        public virtual ICollection<PermisoRol> permisosRol { get; set; } = new List<PermisoRol>();

    }
}
