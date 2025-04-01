using System.Reflection.Metadata;

namespace Geko_MVC_Project.Models

{
    public class VacacionModel
    {
        public int Id { get; set; }
        public string idEmpleado { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaVacacionInicio { get; set; }
        public string FechaVacacionFin { get; set; }
        public string Status { get; set; }
    }
}