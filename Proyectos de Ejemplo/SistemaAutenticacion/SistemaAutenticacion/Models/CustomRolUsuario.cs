using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{
    public class CustomRolUsuario: IdentityRole
    {
        //Clase para el manejo de roles de usuario como Admin, usuario, etc.
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public virtual ICollection<PermisoRol> RolPermisos { get; set; } = new List<PermisoRol>();
    }
}
