using EvaluacionApi.Models;

namespace EvaluacionApi.Services
{
    public class LogService
    {
        private readonly List<LogAccion> _logs = new();

        public void Registrar(string mensaje)
        {
            _logs.Add(new LogAccion { Mensaje = mensaje });
        }

        public List<LogAccion> ObtenerLogs()
        {
            return _logs;
        }
    }
}
