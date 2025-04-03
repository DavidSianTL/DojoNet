using System.ComponentModel.DataAnnotations;
namespace Prueba_de_app_MVC.Models
{

    public class PeticionVacacionesModel
        {
            public int IdPeticion { get; set; }

            [Required(ErrorMessage = "El identificador del empleado es obligatorio")]
            public int IdEmpleado { get; set; }

            [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
            [DataType(DataType.Date, ErrorMessage = "La fecha de inicio debe ser una fecha válida")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime FechaInicio { get; set; }

            [Required(ErrorMessage = "La fecha que finaliza las vacaciones, es obligatoria")]
            [DataType(DataType.Date, ErrorMessage = "La fecha de fin debe ser una fecha válida")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime FechaFin { get; set; }

            public string Estatus { get; set; } = "Pendiente";

            public string Comentarios { get; set; }
    }

}