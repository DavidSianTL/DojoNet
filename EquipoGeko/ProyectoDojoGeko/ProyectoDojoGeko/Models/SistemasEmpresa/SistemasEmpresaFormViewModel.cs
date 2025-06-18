using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoDojoGeko.Models.SistemasEmpresa
{
    public class SistemasEmpresaFormViewModel
    {
        public SistemasEmpresaViewModel SistemasEmpresa { get; set; } = new();
        public List<int> FK_IdsSistema { get; set; } = new(); // << Múltiples sistemas seleccionados
        public List<SelectListItem> Empresas { get; set; } = new();
        public List<SelectListItem> Sistemas { get; set; } = new();
    }
}