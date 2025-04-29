using System;

namespace ExamenUno.Services
{
    public static class LoggerService
    {
        public static void LogError(Exception ex)
        {
            string logRoute = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "Log.txt");

            Directory.CreateDirectory(Path.GetDirectoryName(logRoute));

            string logMessage = $@"
			======================================================================

			Fecha: {DateTime.Now}
			Mensaje: {ex.Message}
			StackTrace: {ex.StackTrace}

			======================================================================
			";
            System.IO.File.AppendAllText(logRoute, logMessage);
        }


    }
}
