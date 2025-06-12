namespace SistemaDeSeguridad.Models
{
    public class PermisoRol
    {
        public string? RolId { get; set; } //FK a CustomRolUsuario
        public int? PermisoId { get; set; } //FK a Permiso

        public CustomRolUsuario? Rol { get; set; }
        public Permisos? Permisos { get; set; }
    }
}
