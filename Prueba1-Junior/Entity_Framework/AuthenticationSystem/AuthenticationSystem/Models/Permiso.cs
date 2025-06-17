using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;


        // Propiedad de navegación: un permiso puede estar asociado a varios roles.
        public virtual ICollection<PermisoRol> permisosRol { get; set; } = new List<PermisoRol>();

    }
}
