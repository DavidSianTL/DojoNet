using System;
using System.IO;

namespace Practica_JavaScript.Services
{
    public class BitacoraService
    {
        private readonly string _logPath;

        public BitacoraService(string logPath)
        {
            _logPath = logPath;
        }

        public void RegistrarAccion(string mensaje)
        {
            var logMessage = $"{DateTime.Now} - {mensaje}{Environment.NewLine}";
            File.AppendAllText(_logPath, logMessage);
        }
    }
}
