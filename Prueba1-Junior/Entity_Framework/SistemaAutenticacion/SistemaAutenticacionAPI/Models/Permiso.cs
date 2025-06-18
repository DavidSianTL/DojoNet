using System.ComponentModel.DataAnnotations;

namespace SistemaAutenticacionAPI.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }
        public string? NombrePermiso { get; set; } //Ej: Crear, Editar, Eliminar, Leer
        public string? Descripcion { get; set; }
        public virtual ICollection<PermisoRol> RolPermisos { get; set; } = new List<PermisoRol>();
    }
}
