namespace Mi_prera_app_MVC.Models
{
    public class SolicitudVacacionesModel
    {
        public string Empleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaFin { get; set; }
        public int status { get; set; }
        public bool Aprobado { get; set; }
    }
}
