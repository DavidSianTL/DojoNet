using System.ComponentModel.DataAnnotations;

namespace SistemaDeSeguridad.Models
{
    public class Permisos
    {
        [Key]
        public int Id { get; set; }
        public string? NombrePermiso { get; set; } //Ej: Crear
        public string? Descripcion { get; set; }

        public virtual ICollection<PermisoRol> RolPermisos { get; set; } = new List<PermisoRol>();
    }
}
