using System.Text;

namespace _Evaluacion_Mensual_Abril.Services
{
    public class LoggerServices
    {
        private readonly string _rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private readonly string _rutaArchivo;

        public LoggerServices()
        {
            
            if (!Directory.Exists(_rutaCarpeta))
            {
                Directory.CreateDirectory(_rutaCarpeta);
            }

            _rutaArchivo = Path.Combine(_rutaCarpeta, "logs.txt");
        }

        public void RegistrarError(string usuario, string mensaje)
        {
            var log = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Usuario: {usuario} - Error: {mensaje}";
            File.AppendAllText(_rutaArchivo, log + Environment.NewLine, Encoding.UTF8);
        }

        public void RegistrarAccion(string usuario, string mensaje)
        {
            var log = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] Usuario: {usuario} - Acción: {mensaje}";
            File.AppendAllText(_rutaArchivo, log + Environment.NewLine, Encoding.UTF8);
        }
    }
}

