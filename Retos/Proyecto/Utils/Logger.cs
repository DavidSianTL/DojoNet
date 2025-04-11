using System;
using System.IO;

namespace Proyecto.Utils
{
    public static class Logger
    {
        // Ruta absoluta a la carpeta App_Data dentro del proyecto
        private static readonly string LogDirectory = Path.Combine(AppContext.BaseDirectory, "App_Data");
        private static readonly string LogPath = Path.Combine(LogDirectory, "log.txt");

        public static void RegistrarError(string accion, Exception ex)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);

                string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Acción: {accion}, Error: {ex.Message}, Detalles: {ex.StackTrace}{Environment.NewLine}";

                File.AppendAllText(LogPath, errorMessage);
                Console.WriteLine($"Error registrado en {LogPath}");
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error al registrar el log: {logEx.Message}");
                Console.WriteLine($"StackTrace: {logEx.StackTrace}");
            }
        }

        public static void RegistrarMensaje(string mensaje)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Mensaje: {mensaje}{Environment.NewLine}";

                File.AppendAllText(LogPath, logEntry);
                Console.WriteLine($"Mensaje registrado en {LogPath}");
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error al registrar mensaje: {logEx.Message}");
                Console.WriteLine($"StackTrace: {logEx.StackTrace}");
            }
        }
    }
}

