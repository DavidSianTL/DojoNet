using System.ComponentModel.DataAnnotations;

namespace SistemaAutenticacion.Models
{
    public class PermisosViewModel
    {

        [Key]
        public int? Id { get; set; } // Llave primaria de la tabla Permisos

        public string? NombrePermiso { get; set; } // Agregar, Editar, Eliminar, Listar, etc.

        public string? Descripcion { get; set; } // Descripcion del permiso

        // Propiedades de navegacion
        public virtual ICollection<PermisosRolViewModel>? PermisosRoles { get; set; } = new List<PermisosRolViewModel>(); // Relacion de la tabla Permisos

    }
}
