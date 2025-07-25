﻿namespace SistemaAutenticacionAPI.Models
{
    public class PermisoRol
    {
        public string? RolId { get; set; } // FK a CustomRolUsuario
        public int? PermisoId { get; set; } // FK a Permiso

        // Propiedades de navegación
        public CustomRolUsuario? Rol { get; set; }
        public Permiso? Permisos { get; set; }
    }
}
