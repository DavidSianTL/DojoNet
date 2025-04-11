namespace MiPrimeraAppMVC.Models
{
    public class SolicitudVacacionesModel
    {
        string? Nombre { get; set; }
        DateTime FechaInicio { get; set; }
        DateTime FechaFin { get; set; }
        int DiasSolicitados { get; set; }
        string Motivo { get; set; }
        bool Aprobada { get; set; }
    


    public SolicitudVacacionesModel(string? nombre, DateTime fechaInicio, DateTime fechaFin, int diasSolicitados, string motivo, bool aprobada)
        {
            Nombre = nombre;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            DiasSolicitados = diasSolicitados;
            Motivo = motivo;
            Aprobada = aprobada;
        }

    }
}
