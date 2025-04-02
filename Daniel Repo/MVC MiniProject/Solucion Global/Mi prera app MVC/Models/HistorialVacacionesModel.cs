namespace Mi_prera_app_MVC.Models
{
    public class HistorialVacacionesModel
    {
        public int id { get; set; }
        public int EmpleadoID { get; set; }
        public string SolicitudID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
