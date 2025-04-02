using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


{
    public class SolicitudVacaciones
    {
        public string Id { get; set; }
        public string IdEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public int DiasSolicitados { get; set; }
        public string Estado { get; set; } // "Pendiente", "Aprobada", "Rechazada"

        public SolicitudVacaciones(string id, string idEmpleado, DateTime fechaInicio, int diasSolicitados)
        {
            Id = id;
            IdEmpleado = idEmpleado;
            FechaInicio = fechaInicio;
            DiasSolicitados = diasSolicitados;
            Estado = "Pendiente";
        }

        public void Aprobar() => Estado = "Aprobada";
        public void Rechazar() => Estado = "Rechazada";
    }
}
