using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Dtos.Empleados.Responses
{
    public class EmpleadoResponse
    {

        [DisplayName("ID")]
        public int IdEmpleado { get; set; }

        [DisplayName("Nombres")]
        public string NombreEmpleado { get; set; }

        [DisplayName("Apellidos")]
        public string ApellidoEmpleado { get; set; }

        [DisplayName("Pais")]
        public string Pais { get; set; }

        [DisplayName("DPI")]
        public string DPI { get; set; }

        [DisplayName("Pasaporte")]
        public string Pasaporte { get; set; }

        [DisplayName("Correo Institucional")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string CorreoInstitucional { get; set; }

        [DisplayName("Fecha de Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        [DisplayName("Género")]
        public string Genero { get; set; }  

        [DisplayName("Antigüedad")]
        public string Antiguedad => CalcularAntiguedad(FechaIngreso);

        [DisplayName("Estado")]
        public int Estado { get; set; }

        [DisplayName("Nombre Completo")]
        public string NombreCompleto => $"{NombreEmpleado} {ApellidoEmpleado}".Trim();

        private string CalcularAntiguedad(DateTime fechaIngreso)
        {
            var diferencia = DateTime.Now - fechaIngreso;
            int años = diferencia.Days / 365;
            int meses = (diferencia.Days % 365) / 30;
            return $"{años} años y {meses} meses";
        }

    }
}
