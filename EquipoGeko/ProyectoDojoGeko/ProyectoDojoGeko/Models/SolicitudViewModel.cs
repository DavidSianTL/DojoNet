using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
    // Modelo para el detalle de una solicitud (cada período de vacaciones)
    [Table("SolicitudDetalle")]
    public class SolicitudDetalleViewModel
    {
        [Column("IdSolicitudDetalle")]
        public int IdSolicitudDetalle { get; set; }

        [Column("FK_IdSolicitud")]
        public int IdSolicitud { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public decimal DiasHabilesTomados { get; set; } = 0.00M;
    }

    // Modelo para el encabezado de la solicitud
    [Table("SolicitudEncabezado")]
    public class SolicitudEncabezadoViewModel
    {
        [Column("IdSolicitud")]
        public int? IdSolicitud { get; set; }

        [Column("FK_IdEmpleado")]
        public int IdEmpleado { get; set; }

        [Column("NombresEmpleado")]
        public string NombreEmpleado { get; set; } = string.Empty;

        [Column("NombreEstado")]
        public string NombreEstado { get; set; }

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

        [Column("MotivoRechazo")]
        public string? MotivoRechazo { get; set; }
    }

    // Clase principal que maneja la solicitud de vacaciones completa
    public class SolicitudViewModel
    {
        // Accedemos al encabezado de la solicitud y a los detalles asociados
        public SolicitudEncabezadoViewModel Encabezado { get; set; } = new SolicitudEncabezadoViewModel();
        public List<SolicitudDetalleViewModel> Detalles { get; set; } = new List<SolicitudDetalleViewModel>();
    }

}
