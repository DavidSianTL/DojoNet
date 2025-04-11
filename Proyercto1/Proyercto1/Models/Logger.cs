using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "error-log.txt");

    // Método para loguear errores
    public static void LogError(string usuario, string mensajeError)
    {
        try
        {
            // Verificamos si el directorio 'logs' existe, si no, lo creamos
            string directory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Abrimos el archivo de log (si no existe, lo creamos)
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                // Escribimos el error con fecha, usuario y mensaje de error
                sw.WriteLine($"Fecha: {DateTime.Now}, Usuario: {usuario}, Error: {mensajeError}");
            }
        }
        catch (Exception ex)
        {
            // Si ocurre un error al registrar el log, lo mostramos en consola
            Console.WriteLine($"No se pudo registrar el error: {ex.Message}");
        }
    }
}
