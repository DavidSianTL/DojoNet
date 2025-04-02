using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


{
    public class ConfiguracionVacaciones
    {
        public int Vacaionesporperido { get; set; }
        public bool PermiteAcumulacion { get; set; }
        public int MaxDiasAcumulables { get; set; }

        public ConfiguracionVacaciones(int Vacaionesporperido, bool permiteAcumulacion, int maxDiasAcumulables)
        {
            Vacaionesporperido = Vacaionesporperido;
            PermiteAcumulacion = permiteAcumulacion;
            MaxDiasAcumulables = maxDiasAcumulables;
        }
        
    }
}