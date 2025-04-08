using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAppMVC.Models
{
    public class SolicitudVacacionesModel
    {
        [Required(ErrorMessage="El campo nombre es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Solo se permiten letras")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [DataType (DataType.DateTime)]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage ="La fecha de fin es requerida")]
        [DataType (DataType.DateTime)]
        public DateTime FechaFin { get; set; }
        public int DiasSolicitados => (FechaFin - FechaInicio).Days; // Calcula los días solicitados
        [Required(ErrorMessage ="El motivo es requerido")]
        public string Motivo { get; set; }
        public bool Aprobada { get; set; }
    


  
        }

    }
