namespace ProyectoDojoGeko.Models.ProyectoEquipo
{
    public class EquipoViewModel
    {
        public int IdEquipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int FK_IdEstado { get; set; }
    }
}
