using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{
    public class PermisosRolViewModel
    {

        public string? RolId { get; set; } // Llave foranea de la tabla Roles

        public int? PermisoId { get; set; } // Llave foranea de la tabla Permisos

        // Propiedades de navegacion
        public CustomRolUsuarioViewModel? Rol { get; set; } // Relacion de la tabla Roles

        public PermisosViewModel? Permiso { get; set; } // Relacion de la tabla Permisos


    }
}
