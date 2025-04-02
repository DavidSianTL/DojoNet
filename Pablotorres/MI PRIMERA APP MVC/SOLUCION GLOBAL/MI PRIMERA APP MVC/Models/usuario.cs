namespace MI_PRIMERA_APP_MVC.Models
{
    public class USUARIO_MODEL
    {
        public int id {  get; set; }
        public string nombre {  get; set; }
        public string Apellido { get; set; }
        public string username { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int status { get; set; }
        int vacaciones { get; set; }
        public string proyectos { get; set; }
        public string prioridad { get; set; }
    }
}
