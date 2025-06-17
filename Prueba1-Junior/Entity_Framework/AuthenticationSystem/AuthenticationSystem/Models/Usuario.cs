using Microsoft.AspNetCore.Identity;

namespace AuthenticationSystem.Models
{
    public class Usuario : IdentityUser
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Telefono { get; set; } = string.Empty;
        public string FotoPerfilURL { get; set; } = string.Empty;
        public DateTime FechaUltimoLogin { get; set; } = DateTime.UtcNow;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaNacimiento { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación: un usuario puede tener varios roles.
        public virtual ICollection<Rol> Roles { get; set; } = new List<Rol>();
    }
}
