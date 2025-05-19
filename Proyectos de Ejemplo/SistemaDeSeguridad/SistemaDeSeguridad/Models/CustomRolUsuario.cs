using Microsoft.AspNetCore.Identity;

namespace SistemaDeSeguridad.Models
{
    public class CustomRolUsuario: IdentityRole
    {
        //Clase para el manejo de roles de ususario como Administrador o Usuario
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public virtual ICollection<PermisoRol> RolPermisos { get; set; } = new List<PermisoRol>();
    }
}
