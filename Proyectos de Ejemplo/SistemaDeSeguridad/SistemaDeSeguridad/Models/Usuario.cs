using Microsoft.AspNetCore.Identity;

namespace SistemaDeSeguridad.Models
{
    public class Usuario: IdentityUser
    {
        //Propiedades basicas
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? NombreCompleto => $"{Nombre} {Apellido}";
        public string? Telefono { get; set; }
        public string? FotoPerfilURL { get; set; }
        public DateTime? FechaUltimoLogin { get; set; }
        public DateTime? FechaCreacion { get; set; }

        //Relacion con CustomRolUsuario
        public ICollection<CustomRolUsuario>? Roles { get; set; }
    }
}
