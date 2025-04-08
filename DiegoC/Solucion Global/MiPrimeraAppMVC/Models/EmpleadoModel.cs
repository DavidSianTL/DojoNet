namespace MiPrimeraAppMVC.Models
{
    public class EmpleadoModel
    {
        
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string CorreoElectronico { get; set; }
            public string Telefono { get; set; }
            public DateTime FechaContratacion { get; set; }
            public int DiasVacacionesDisponibles { get; set; }
            public string Puesto { get; set; }

        }
    }
