using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoDojoGeko.Models.Usuario
{
    // Esta clase se utiliza para crear un nuevo usuario
    public class UsuarioFormViewModel
    {
        // Esta propiedad representa el modelo de usuario que se va a crear
        public UsuarioViewModel Usuario { get; set; } = new();

        // Esta lista se utiliza para mostrar los empleados disponibles en un dropdown
        public List<SelectListItem> Empleados { get; set; } = new();
    }
}
