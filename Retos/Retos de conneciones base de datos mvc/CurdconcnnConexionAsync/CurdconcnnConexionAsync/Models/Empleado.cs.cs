namespace CurdconcnnConexionAsync.Models
{
    public class Empleado
    {
        public int EmpleadoID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Puesto { get; set; }
        public decimal SalarioBase { get; set; }
        public bool Activo { get; set; }

        public Empleado() { }
    }
}
