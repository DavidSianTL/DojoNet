using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Dtos.Solicitudes
{
    public class SolicitudEncabezadoResult
    {
        
        [Column("IdSolicitud")]
        public int? IdSolicitud { get; set; }

        [Column("FK_IdEmpleado")]
        public int IdEmpleado { get; set; }

        [Column("NombresEmpleado")]
        public string NombreEmpleado { get; set; } = string.Empty;
       
        [Column("NombreEstado")]
        public string NombreEstado { get; set; } = string.Empty;

        [Column("NombreEmpresa")]
        public string NombreEmpresa { get; set; } = string.Empty;

        [Column("DiasSolicitadosTotal")]
        public decimal DiasSolicitadosTotal { get; set; } = 0.00M;

        [Column("FechaIngresoSolicitud")]
        public DateTime FechaIngresoSolicitud { get; set; }

        [Column("SolicitudLider")]
        public string SolicitudLider { get; set; }

        [Column("Observaciones")]
        public string Observaciones { get; set; }

        [Column("FK_IdEstadoSolicitud")]
        public int Estado { get; set; }

        [Column("FK_IdAutorizador")]
        public int? IdAutorizador { get; set; }

        [Column("FechaAutorizacion")]
        public DateTime? FechaAutorizacion { get; set; }
        
        [Column("FechaInicio")]
        public DateTime? FechaInicio { get; set; }
        
        [Column("FechaFin")]
        public DateTime? FechaFin { get; set; }

        [Column("MotivoRechazo")]
        public string? MotivoRechazo { get; set; }
        
    }
}
