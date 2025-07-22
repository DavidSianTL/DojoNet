using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ProyectoDojoGeko.Models
{
    // Modelo para el detalle de una solicitud (cada período de vacaciones)
    public class SolicitudDetalleViewModel
    {
        public int IdSolicitudDetalle { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "Los días hábiles son obligatorios.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe solicitar al menos 1 día hábil.")]
        public int DiasHabilesTomados { get; set; }
    }

    // Modelo para el encabezado de la solicitud
    public class SolicitudEncabezadoViewModel
    {
        public int IdSolicitud { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } // Para mostrar en la vista
        public int DiasSolicitadosTotal { get; set; }
        public DateTime FechaIngresoSolicitud { get; set; }
        public string Estado { get; set; } // Nombre del estado para mostrar

        // Lista para contener todos los detalles asociados
        public List<SolicitudDetalleViewModel> Detalles { get; set; } = new List<SolicitudDetalleViewModel>();
    }

    public class SolicitudViewModel
    {
    }
}
