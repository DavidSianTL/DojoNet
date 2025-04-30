using System;
using System.IO;

namespace Examen_mes_abril.Services
{
    public static class BitacoraService
    {
        private static readonly string rutaArchivo = "Logs/bitacora.txt";

        static BitacoraService()
        {
            Directory.CreateDirectory("Logs");
        }

        public static void RegistrarEvento(string tipo, string mensaje)
        {
            string entrada = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{tipo.ToUpper()}] {mensaje}";
            File.AppendAllLines(rutaArchivo, new[] { entrada });
        }

        public static string[] ConsultarBitacora()
        {
            if (!File.Exists(rutaArchivo))
                return new[] { "No hay registros en la bitácora." };

            return File.ReadAllLines(rutaArchivo);
        }
    }
}