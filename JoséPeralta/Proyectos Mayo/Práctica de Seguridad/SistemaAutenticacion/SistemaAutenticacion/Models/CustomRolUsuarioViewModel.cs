using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{

    public class CustomRolUsuarioViewModel : IdentityRole
    {

        public string? Descripcion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        // Relacion de las tablas
        public virtual ICollection<UsuarioViewModel>? Usuarios { get; set; }


    }
}
