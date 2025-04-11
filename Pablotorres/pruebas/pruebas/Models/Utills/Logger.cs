using System;
using System.IO;

namespace pruebas.Utils
{
    public static class Logger
    {
        private static readonly string logFilePath = "logs.txt"; 

        public static void LogError(string usuario, string mensaje)
        {
            try
            {
                string log = $"[{DateTime.Now}] Usuario: {usuario} - Error: {mensaje}";
                File.AppendAllText(logFilePath, log + Environment.NewLine);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error al escribir en el log: " + ex.Message);
            }
        }
    }
}
