using Microsoft.AspNetCore.Mvc;

namespace EvaluacionApi.Models
{
    public class LogAccion
    {
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Mensaje { get; set; } = string.Empty;
    }
}
