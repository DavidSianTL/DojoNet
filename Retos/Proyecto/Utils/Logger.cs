using System;
using System.IO;

namespace Proyecto.Utils
{
    public static class Logger
    {
        private static readonly string logPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "logs.txt");

        public static void RegistrarAccion(string usuario, string accion)
        {
            try
            {
                string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Usuario: {usuario} | Acción: {accion}";
                File.AppendAllText(logPath, log + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Manejar error si lo deseas
            }
        }

        public static void RegistrarError(string contexto, Exception ex)
        {
            try
            {
                string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | ERROR en {contexto}: {ex.Message}\n{ex.StackTrace}";
                File.AppendAllText(logPath, log + Environment.NewLine);
            }
            catch
            {
                // Silenciar errores de log
            }
        }
    }
}
