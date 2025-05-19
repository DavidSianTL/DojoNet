using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{

    public class UsuarioViewModel : IdentityUser
    {

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }

        public string? NombreCompleto => $"{Nombres} {Apellidos}";

        public string? Telefono { get; set; }

        public string? FotoPerfilUrl { get; set; }

        public DateTime? FechaUltimoAcceso { get; set; }

        public DateTime? FechaCreacion { get; set; }

        // Relacion de las tablas
        public ICollection<CustomRolUsuarioViewModel>? Roles { get; set; }




    }
}
