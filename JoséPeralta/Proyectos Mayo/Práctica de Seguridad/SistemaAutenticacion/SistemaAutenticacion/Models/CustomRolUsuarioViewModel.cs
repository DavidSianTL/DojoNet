using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{

    public class CustomRolUsuarioViewModel : IdentityRole
    {

        //Clase para el manejo de roles de usuario como Admin, usuario, etc.
        public string? Descripcion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        // Relacion de las tablas
        public virtual ICollection<PermisosRolViewModel>? RolPermisos { get; set; } = new List<PermisosRolViewModel>(); // Relacion de la tabla AspNetRoles


    }
}
