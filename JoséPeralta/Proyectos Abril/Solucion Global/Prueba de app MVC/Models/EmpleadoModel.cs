namespace Prueba_de_app_MVC.Models
{
    public class EmpleadoModel
    {
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime DiasVacacionesDisponibles { get; set; }

        public string Status { get; set; }

        public string Puesto { get; set; }

    }
}
