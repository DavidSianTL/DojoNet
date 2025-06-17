using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{
    public class Usuarios: IdentityUser
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreCompleto => $"{Nombre} {Apellido}";
        public string? Telefono { get; set; }
        public string? FotoPerfilURL { get; set; }
        public DateTime? FechaUltimoLogin { get; set; }
        public DateTime? FechaCreacion { get; set; }

        //Relaciones
        public ICollection<CustomRolUsuario>? Roles { get; set; }
    }
}
