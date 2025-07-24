using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.Empleados
{
    public class EmpleadosEmpresaDepartamentoFormViewModel
    {

        // Esta propiedad representa el modelo de usuario que se va a crear
        public EmpleadosDepartamentoViewModel EmpleadosDepartamento { get; set; } = new();

        // Esta propiedad representa el ID del empleado que se va a crear
        public EmpleadosEmpresaViewModel EmpleadosEmpresa { get; set; } = new();

        // Esta lista se utiliza para mostrar las empresas disponibles en un dropdown
        public List<SelectListItem> Empresas { get; set; } = new();

        // Esta lista se utiliza para mostrar los empleados disponibles en un dropdown
        public List<SelectListItem> Empleados { get; set; } = new();

        [NotMapped]
        public List<int> FK_IdsEmpleado { get; set; } = new();

        // Esta lista se utiliza para mostrar los departamentos disponibles en un dropdown
        public List<SelectListItem> Departamentos { get; set; } = new();


    }
}
