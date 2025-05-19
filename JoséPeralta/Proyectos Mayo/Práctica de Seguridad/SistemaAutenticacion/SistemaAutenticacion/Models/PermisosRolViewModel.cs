using Microsoft.AspNetCore.Identity;

namespace SistemaAutenticacion.Models
{
    public class PermisosRolViewModel
    {

        public string? RoldId { get; set; } // Llave foranea de la tabla AspNetRoles

        public int? PermisoId { get; set; } // Llave foranea de la tabla Permisos

        // Propiedades de navegacion
        public CustomRolUsuarioViewModel? Rol { get; set; } // Relacion de la tabla AspNetRoles

        public PermisosViewModel? Permiso { get; set; } // Relacion de la tabla Permisos


    }
}
