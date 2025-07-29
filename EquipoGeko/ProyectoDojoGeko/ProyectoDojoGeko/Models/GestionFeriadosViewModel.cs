using System.Collections.Generic;

namespace ProyectoDojoGeko.Models
{
    public class GestionFeriadosViewModel
    {
        public IEnumerable<FeriadoFijoViewModel> FeriadosFijos { get; set; }
        public IEnumerable<FeriadoVariableViewModel> FeriadosVariables { get; set; }
        public IEnumerable<TipoFeriadoViewModel> TiposFeriado { get; set; }

        public GestionFeriadosViewModel()
        {
            FeriadosFijos = new List<FeriadoFijoViewModel>();
            FeriadosVariables = new List<FeriadoVariableViewModel>();
            TiposFeriado = new List<TipoFeriadoViewModel>();
        }
    }
}
