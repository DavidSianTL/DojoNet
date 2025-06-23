using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoDojoGeko.Models.DepartamentosEmpresa
{
    public class DepartamentosEmpresaFormViewModel
    {
        // Le pasamos los parámetros necesarios para asignar departamentos a una empresa
        // en este caso, el ID de la empresa y una lista de IDs de departamentos seleccionados.
        public int FK_IdEmpresa { get; set; }
        public List<int> FK_IdsDepartamentos { get; set; } = new(); // << Múltiples permisos seleccionados

        // Listas para los selectores de empresas y departamentos
        public List<SelectListItem> Empresas { get; set; } = new();
        public List<SelectListItem> Departamentos { get; set; } = new();

    }
}
