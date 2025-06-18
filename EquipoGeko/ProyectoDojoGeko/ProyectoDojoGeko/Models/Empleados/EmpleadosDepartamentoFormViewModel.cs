using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Models.Empleados
{
    public class EmpleadosDepartamentoFormViewModel
    {

        // Esta propiedad representa el modelo de usuario que se va a crear
        public EmpleadosDepartamentoViewModel EmpleadosDepartamento { get; set; } = new();

        // Esta lista se utiliza para mostrar los empleados disponibles en un dropdown
        public List<SelectListItem> Empleados { get; set; } = new();

        // Esta lista se utiliza para mostrar los departamentos disponibles en un dropdown
        public List<SelectListItem> Departamentos { get; set; } = new();

    }
}
