namespace ProyectoDojoGeko.Models.ProyectoEquipo
{
    public class ProyectoViewModel
    {
        public int IdProyecto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public int FK_IdEstado { get; set; }
    }
}
