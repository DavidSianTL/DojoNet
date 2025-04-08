namespace wAppGestionVacacional.Models
{
    public class Empleado
    {
           
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos{ get; set; }
        public string Puesto { get; set; }
        public DateTime FechaIngreso { get; set; }

        public int DiasVacacionesDisponibles { get; set; } = 15; // Asumimos 15 días por defecto



      /*  public Empleado(int id, string nombres, string apellidos,string puesto, DateTime fechaingreso)
        {
            Id = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Puesto = puesto;
            FechaIngreso = fechaingreso;
        }*/
        // Método para calcular los días de vacaciones según la fecha de ingreso
        public void CalcularDiasVacaciones()
        {
            var añosTrabajados = DateTime.Now.Year - FechaIngreso.Year;
            if (DateTime.Now < FechaIngreso.AddYears(añosTrabajados))
            {
                añosTrabajados--;
            }
            DiasVacacionesDisponibles = añosTrabajados * 15;
        }


    }
}

