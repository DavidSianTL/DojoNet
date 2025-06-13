using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoDojoGeko.Models.RolPermisos
{
    public class RolPermisosFormViewModel
    {
        public int FK_IdRol { get; set; }

        public int FK_IdSistema { get; set; }

        public List<int> FK_IdsPermisos { get; set; } = new(); // << Múltiples permisos seleccionados

        public List<SelectListItem> Permisos { get; set; } = new();
        public List<SelectListItem> Sistemas { get; set; } = new();
        public List<SelectListItem> Roles { get; set; } = new();
    }
}