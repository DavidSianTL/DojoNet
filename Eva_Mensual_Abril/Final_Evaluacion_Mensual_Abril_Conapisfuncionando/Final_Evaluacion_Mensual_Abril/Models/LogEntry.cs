using System;

namespace Final_Evaluacion_Mensual_Abril.Models
{
    public class LogEntry
    {
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Usuario { get; set; }
        public string Accion { get; set; } 
        public string Controlador { get; set; }
        public string Descripcion { get; set; }
        public bool EsError { get; set; } = false;

       
    }
}