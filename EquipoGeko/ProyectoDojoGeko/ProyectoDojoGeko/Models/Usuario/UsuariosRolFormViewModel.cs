using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoDojoGeko.Models.Usuario
{
    public class UsuariosRolFormViewModel
    {
        // Le pasamos los parámetros necesarios para asignar roles a un usuario
        // en este caso, el ID del usuario y una lista de IDs de roles seleccionados.
        public int FK_IdUsuario { get; set; }
        public List<int> FK_IdsRol { get; set; } = new(); // << Múltiples permisos seleccionados

        // Listas para los selectores de roles y usuarios
        public List<SelectListItem> Usuarios { get; set; } = new();
        public List<SelectListItem> Roles { get; set; } = new();

    }
}
